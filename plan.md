# Plan for multi-database stress support

## Current status

This is **not currently supported** inside a single SqlQueryStress run.

Today, both the WinForms app and the CLI load a single `MainDbConnectionInfo.Database` value and build one target connection string from it. `LoadEngine` then creates all worker connections from that same connection string, so every thread runs against the same database.

Current workaround: run multiple WinForms or CLI instances concurrently, each pointed at a different database.

## Why this enhancement needs design work

The original issue pre-dates the CLI, but the current repository now has two entry points that share the same runtime classes:

- WinForms app selects one database in `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/DatabaseSelect.cs`
- CLI loads one database from JSON in `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SqlQueryStressCLI/sample.json`
- Shared connection model uses one `Database` property in `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/ConnectionInfo.cs`
- Shared execution engine uses one connection string in `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/LoadEngine.cs`

Because the engine is shared, a good solution should be designed once in the shared model/runtime and then surfaced separately in WinForms and CLI.

## Recommended design

### 1. Preserve backward compatibility

Keep the existing single-database `Database` setting working unchanged.

Add a new optional collection-based setting for target databases, for example on `QueryStressSettings` rather than replacing `ConnectionInfo.Database` directly. That avoids breaking saved settings and keeps the connection object usable anywhere a single database is still required.

Proposed behavior:

- If no database list is provided, run exactly as today using `MainDbConnectionInfo.Database`
- If a database list is provided, ignore the single `Database` value for main-query execution and distribute workers across the listed databases

### 2. Put multi-database intent in settings, not in the raw connection object

Prefer a setting such as:

- `MainDatabases: string[]` or `List<string>`

on `QueryStressSettings`.

Why:

- The current `ConnectionInfo` maps cleanly to one `SqlConnectionStringBuilder.InitialCatalog`
- Parameter-fetch connections may still need to remain single-database
- This reduces ripple effects in connection testing, cloning, and UI code

### 3. Distribute threads deterministically across databases

Extend `LoadEngine` so it can accept either:

- one connection string, or
- a list of connection strings derived from one base `ConnectionInfo` plus multiple database names

Recommended distribution rule:

- Assign thread `i` to database `i % databaseCount`

This gives a predictable, even spread such as:

- 8 threads across 4 databases => 2 threads per database
- 5 threads across 2 databases => 3 threads on DB1, 2 threads on DB2

Each worker thread should keep using one target database for its lifetime. That keeps connection pooling simple and avoids changing databases mid-thread.

### 4. Keep parameter sourcing independent

Do **not** automatically spread `ParamDbConnectionInfo` across the main database list.

Continue current behavior:

- `ShareDbSettings = true` means parameter retrieval uses the main connection settings
- `ShareDbSettings = false` means parameter retrieval uses `ParamDbConnectionInfo`

For the first version of the feature, parameter retrieval should still come from one database connection. That keeps the feature focused on stressing multiple target databases concurrently.

### 5. CLI design

Because the CLI did not exist when the issue was opened, any new design should add CLI support explicitly.

Recommended CLI-compatible configuration shape:

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

Guidance:

- Keep `Database` in sample/config docs for backward compatibility
- Document that `MainDatabases` overrides the single database when supplied
- Avoid adding a complex new command-line switch initially; JSON configuration is already the CLI’s primary input model

### 6. WinForms design

Update the database selection dialog to optionally capture multiple databases.

Lowest-risk UI direction:

- Keep the current single-database combo box behavior as the default
- Add an advanced option for multiple database names, such as a multiline textbox or checked list
- Reuse the current database discovery query (`sys.databases`) to help populate the list

A multiline textbox may be the smallest UI change because it avoids redesigning the dialog around multi-select controls.

### 7. Validation and guardrails

Add validation before starting a run:

- Reject an empty database list after trimming
- Reject duplicate database names after normalization
- Ensure every configured target database can produce a valid connection string
- Report clearly how threads will be distributed

Manual verification scenarios to cover when implementing later:

1. Single database only => behavior unchanged
2. Two databases with four threads => both databases receive work
3. More databases than threads => first `threadCount` databases are used once
4. Blank/duplicate names in list => validation error
5. CLI JSON without `MainDatabases` => legacy behavior unchanged

## Suggested implementation order

1. Add a backward-compatible multi-database setting to `QueryStressSettings`
2. Update serialization/sample configuration documentation
3. Extend `LoadEngine` to build per-thread connections from a database list
4. Update CLI messaging/documentation
5. Update WinForms database-selection UX
6. Manually verify single-database and multi-database runs

## Files most likely involved in a future implementation

- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/QueryStressSettings.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/ConnectionInfo.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/LoadEngine.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/DatabaseSelect.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SQLQueryStress/FormMain.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SqlQueryStressCLI/Program.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SqlQueryStressCLI/LoadRunner.cs`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SqlQueryStressCLI/sample.json`
- `/home/runner/work/SqlQueryStress/SqlQueryStress/src/SqlQueryStressCLI/README.md`

## Build/test notes for a future implementation

- CLI build verified locally with:
  - `dotnet build ./src/SqlQueryStressCLI/sqlstresscmd.csproj -c Release`
- Full solution build is defined in GitHub Actions on Windows:
  - `dotnet build ./src/SQLQueryStress.sln --configuration Release`
- No test project currently exists in the solution, so verification would likely be manual unless tests are introduced separately.
