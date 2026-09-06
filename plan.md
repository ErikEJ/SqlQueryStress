# Plan for multi-database stress support

## Current status

This is **not currently supported** inside a single SqlQueryStress run.

Today, both the WinForms app and the CLI load a single `MainDbConnectionInfo.Database` value and build one target connection string from it. `LoadEngine` then creates all worker connections from that same connection string, so every thread runs against the same database.

Current workaround: run multiple WinForms or CLI instances concurrently, each pointed at a different database.

## Why this enhancement needs design work

The original issue pre-dates the CLI, but the current repository now has two entry points that share the same runtime classes:

- WinForms app selects one database in `src/SQLQueryStress/DatabaseSelect.cs`
- CLI loads one database from JSON in `src/SqlQueryStressCLI/sample.json`
- Shared connection model uses one `Database` property in `src/SQLQueryStress/ConnectionInfo.cs`
- Shared execution engine uses one connection string in `src/SQLQueryStress/LoadEngine.cs`

Because the engine is shared, a good solution should be designed once in the shared model/runtime and then surfaced separately in WinForms and CLI.

## Recommended design

### 1. Preserve backward compatibility

Keep the existing single-database `Database` setting working unchanged.

Add a new optional collection-based setting for target databases, for example on `QueryStressSettings` rather than replacing `ConnectionInfo.Database` directly. That avoids breaking saved settings and keeps the connection object usable anywhere a single database is still required.

Proposed behavior:

- If no database list is provided, run exactly as today using `MainDbConnectionInfo.Database`
- If a database list is provided, ignore the single `Database` value for main-query execution and distribute workers across the listed databases

### 2. Put multi-target intent in settings, not in the raw connection object

For the original same-server scenario, a setting such as:

- `MainDatabases: string[]` or `List<string>`

on `QueryStressSettings` is sufficient.

If support for multiple servers should be included, the more general shape is an optional target list such as:

- `MainTargets: [{ Server, Database }]`

with `MainDbConnectionInfo` supplying shared defaults like authentication, encryption, timeout, pooling, and additional parameters unless a target overrides them explicitly.

Why:

- The current `ConnectionInfo` maps cleanly to one `SqlConnectionStringBuilder.InitialCatalog`
- A target list can represent either multiple databases on one server or databases across multiple servers
- Parameter-fetch connections may still need to remain single-database
- This reduces ripple effects in connection testing, cloning, and UI code

### 3. Distribute threads deterministically across targets

Extend `LoadEngine` so it can accept either:

- one connection string,
- a list of connection strings derived from one base `ConnectionInfo` plus multiple database names, or
- a list of connection strings derived from a target list containing `Server` + `Database` pairs

Recommended distribution rule:

- Assign thread `i` to target `i % targetCount`

This gives a predictable, even spread such as:

- 8 threads across 4 databases => 2 threads per database
- 5 threads across 2 databases => 3 threads on DB1, 2 threads on DB2

Each worker thread should keep using one resolved target connection for its lifetime. That keeps connection pooling simple and avoids changing databases or servers mid-thread.

### 4. Keep parameter sourcing independent

Do **not** automatically spread `ParamDbConnectionInfo` across the main database list.

Continue current behavior:

- `ShareDbSettings = true` means parameter retrieval uses the main connection settings
- `ShareDbSettings = false` means parameter retrieval uses `ParamDbConnectionInfo`

For the first version of the feature, parameter retrieval should still come from one database connection. That keeps the feature focused on stressing multiple target databases concurrently.

### 5. CLI design

Because the CLI did not exist when the issue was opened, any new design should add CLI support explicitly.

Recommended CLI-compatible configuration shapes:

Same server, multiple databases:

```json
{
  "MainDbConnectionInfo": {
    "Server": "server-name",
    "Database": "db1"
  },
  "MainDatabases": ["db1", "db2", "db3"],
  "NumThreads": 6
}
```

Multiple servers and databases:

```json
{
  "MainDbConnectionInfo": {
    "IntegratedAuth": true,
    "EncryptOption": "Mandatory"
  },
  "MainTargets": [
    { "Server": "server-a", "Database": "db1" },
    { "Server": "server-b", "Database": "db2" }
  ],
  "NumThreads": 6
}
```

Guidance:

- Keep `Database` in sample/config docs for backward compatibility
- Document that `MainDatabases` overrides the single database when supplied
- If `MainTargets` is introduced, document that it overrides both the single `Database` value and any same-server `MainDatabases` list
- Avoid adding a complex new command-line switch initially; JSON configuration is already the CLI’s primary input model

### 6. WinForms design

Update the database selection dialog to optionally capture multiple targets.

Lowest-risk UI direction:

- Keep the current single-database combo box behavior as the default
- Add an advanced option for multiple database names on the current server, such as a multiline textbox or checked list
- If multi-server support is desired, allow one `server,database` pair per line in the advanced UI
- Reuse the current database discovery query (`sys.databases`) to help populate same-server database lists

A multiline textbox may be the smallest UI change because it avoids redesigning the dialog around multi-select controls.

### 7. Validation and guardrails

Add validation before starting a run:

- Reject an empty database list after trimming
- Reject duplicate database names after normalization
- Reject duplicate `server + database` targets after normalization if multi-server support is enabled
- Ensure every configured target can produce a valid connection string
- Report clearly how threads will be distributed

Manual verification scenarios to cover when implementing later:

1. Single database only => behavior unchanged
2. Two databases with four threads => both databases receive work
3. More databases than threads => first `threadCount` databases are used once
4. Blank/duplicate names in list => validation error
5. CLI JSON without `MainDatabases` => legacy behavior unchanged
6. Multiple `MainTargets` entries across different servers => work is distributed across server/database pairs

## Suggested implementation order

1. Add a backward-compatible multi-database setting to `QueryStressSettings`
2. Update serialization/sample configuration documentation
3. Extend `LoadEngine` to build per-thread connections from a database list
4. Update CLI messaging/documentation
5. Update WinForms database-selection UX
6. Manually verify single-database and multi-database runs

## Files most likely involved in a future implementation

- `src/SQLQueryStress/QueryStressSettings.cs`
- `src/SQLQueryStress/ConnectionInfo.cs`
- `src/SQLQueryStress/LoadEngine.cs`
- `src/SQLQueryStress/DatabaseSelect.cs`
- `src/SQLQueryStress/FormMain.cs`
- `src/SqlQueryStressCLI/Program.cs`
- `src/SqlQueryStressCLI/LoadRunner.cs`
- `src/SqlQueryStressCLI/sample.json`
- `src/SqlQueryStressCLI/README.md`

## Build/test notes for a future implementation

- CLI build command:
  - `dotnet build ./src/SqlQueryStressCLI/sqlstresscmd.csproj -c Release`
- Full solution build command used by GitHub Actions on Windows:
  - `dotnet build ./src/SQLQueryStress.sln --configuration Release`
- No test project is currently referenced by `src/SQLQueryStress.sln`, so verification for this feature would likely be manual unless dedicated tests are added later.
