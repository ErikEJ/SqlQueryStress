# Feasibility: New `sqlstresscmd` CLI features

Investigation for [#222](https://github.com/ErikEJ/SqlQueryStress/issues/222): **result export**,
**baselining**, and **CI regression testing** of query performance.

This document assesses how hard each feature is to build on top of the current code base, proposes a
concrete design, and recommends an incremental delivery order. It is a plan only — no runtime behaviour
is changed by this document.

## 1. Current state

The cross-platform `sqlstresscmd` tool reuses the same load engine as the Windows GUI:

- `src/SqlQueryStressCLI/Program.cs` parses options (via `CommandLineParser`), loads a JSON settings
  file (`QueryStressSettings`) and hands control to `LoadRunner`.
- `src/SqlQueryStressCLI/LoadRunner.cs` drives `SQLQueryStress.LoadEngine`, aggregates results and prints
  a Spectre.Console summary table.
- `src/SQLQueryStress/LoadEngine.cs` runs the queries and reports per-iteration `QueryOutput`
  (`CpuTime`, `ElapsedTime`, `LogicalReads`, `Time`, exceptions, `ActiveThreads`).

### Metrics already aggregated in `LoadRunner`

| Metric | Field |
| --- | --- |
| Test id / start time | `_testGuid`, `_testStartTime` |
| Elapsed (wall-clock) time | `theTime` |
| Threads | `_numThreads` |
| Completed iterations | `_totalIterations` |
| Avg CPU seconds / iteration | `cpuTime` |
| Avg actual (SQL elapsed) seconds / iteration | `actualSeconds` |
| Avg client seconds / iteration | `avgSeconds` |
| Avg logical reads / iteration | `logicalReads` |
| Exceptions (count, grouped by message) | `_totalExceptions`, `_exceptions` |

### What already exists for "result export"

Result export is **partially implemented today**. The `-r` / `--results` option
(`CommandLineOptions.ResultsAutoSaveFileName`) triggers `LoadRunner.AutoSaveResults`, which appends a CSV
row (`ExportBenchMarkToCsvFile` / `WriteBenchmarkCsvText`) with the metrics above when the file extension is
`.csv`. Note:

- The option is still annotated `//TODO Implement` in `CommandLineOptions.cs` even though `LoadRunner`
  handles it, so the first task is simply to finish and document it.
- Only `.csv` is handled; any other extension is silently ignored.
- Only **averages** are exported — no percentiles, min/max, or per-iteration detail.
- The CSV is written with the current culture (`_testStartTime`, raw numbers), which is fragile for machine
  parsing.

This existing code is the natural foundation for all three requested features.

## 2. Feature 1 — Result export

### Goal

Emit a complete, machine-readable summary of a run so results can be archived, diffed, and consumed by
other tooling (dashboards, CI, baselining).

### Design

1. Introduce a small immutable `RunResult` record (TestId, StartTime (UTC/ISO-8601), ElapsedTime,
   Threads, Iterations, CompletedIterations, Delay, AvgCpuSeconds, AvgActualSeconds, AvgClientSeconds,
   AvgLogicalReads, ExceptionCount). Populate it in `LoadRunner` instead of formatting strings only for the
   console.
2. Add a `--format` option (`csv` | `json`) and/or infer from the file extension (as the CSV path already
   does). JSON should reuse the existing `JsonSerializer` helper that is already linked into the CLI project.
3. Keep the current CSV append behaviour (useful for trend files) and add a JSON writer.
4. Write numbers and timestamps with `CultureInfo.InvariantCulture` / ISO-8601 for stable machine parsing.

### Optional enhancements

- Capture percentiles (p50/p95/p99), min/max and standard deviation. The engine currently only keeps running
  totals, so this requires retaining per-iteration samples (one `double`/`int` list per metric). For typical
  iteration counts this is cheap; for very large runs it can be bounded with a streaming percentile estimator.

### Effort / risk

**Low.** Mostly additive, reuses existing export path and `JsonSerializer`. Main care points: culture-invariant
formatting and not breaking the existing CSV column layout (append a new format rather than reshaping the old one).

## 3. Feature 2 — Baselining

### Goal

Save a run as a named **baseline**, then compare a later run against it and report deltas (absolute and %).

### Design

1. Reuse the `RunResult` record from Feature 1 as the on-disk baseline format (JSON), e.g.
   `sqlstresscmd -s test.json --save-baseline baseline.json`.
2. Add `--baseline <file>` to load a previous result and, after the run, print a comparison table
   (current vs baseline vs delta/%). Spectre.Console already provides the table rendering.
3. Define which metrics are compared and the direction that is "worse" (higher CPU/elapsed/reads = regression).

### Effort / risk

**Low–Medium.** The comparison and rendering are straightforward once `RunResult` serialization exists.
The main design decisions are which metrics are authoritative and how to normalize across different thread/iteration
counts (recommend comparing per-iteration averages, which are already thread-count independent).

## 4. Feature 3 — CI regression testing

### Goal

Let a CI pipeline fail when query performance regresses beyond a configured threshold.

### Design

1. Build on Features 1 & 2: run, compare against a committed baseline, decide pass/fail.
2. Add threshold options, e.g. `--fail-on-regression` plus per-metric tolerances
   (`--max-cpu-regression 10%`, `--max-elapsed-regression 10%`, or a single overall tolerance).
3. **Return a non-zero process exit code** on regression or on any SQL exception. Today `Program.Main`
   returns `void` and never sets an exit code, so CI cannot detect failures — changing `Main` to return `int`
   (or setting `Environment.ExitCode`) is the key enabling change and is backward compatible for success cases.
4. Emit a concise, parseable summary (the JSON/CSV from Feature 1) as a build artifact.
5. Document a sample GitHub Actions job (mirroring `.github/workflows/dotnet.yml`) that installs the tool,
   runs against a SQL Server service container, and compares to a checked-in baseline.

### Effort / risk

**Medium.** The code change is small (exit codes + threshold evaluation), but a trustworthy regression gate
depends on run-to-run stability. Recommendations: require a minimum iteration count, compare averages/percentiles
rather than single runs, support warm-up iterations, and default tolerances generously to avoid flaky builds.
CI environments are inherently noisy, so this feature should be documented as "indicative" rather than exact.

## 5. Recommended delivery order

1. **Finish & formalize result export** (remove the stale TODO, add JSON, invariant formatting, document `-r`).
   Low risk, immediately useful, and the data model unblocks everything else.
2. **Baselining** (save/compare using the same `RunResult`), reusing Spectre.Console for the diff table.
3. **CI regression testing** (exit codes + thresholds + sample workflow) on top of baselining.

Each step is additive and backward compatible with current CLI behaviour. The single most valuable enabling
change is introducing a shared `RunResult` model and making `Program.Main` return a meaningful exit code; both
are small and unlock the later features.

## 6. Conclusion

All three features are **feasible and low-to-medium effort**. The CLI already collects the needed metrics and
has a working (if minimal) CSV export path, so the work is primarily: (a) formalize export into a structured,
culture-invariant model, (b) add save/compare baselining, and (c) add exit codes and thresholds for CI. No new
heavy dependencies are required — `CommandLineParser`, `Spectre.Console`, and the existing `JsonSerializer` cover
the needs.
