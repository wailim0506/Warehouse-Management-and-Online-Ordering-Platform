# WinForms performance

This refinement targets confirmed sources of general slowness: unnecessary database connections at startup, per-card database reads, blocking login work, and resources retained after refreshes and uploads. It preserves .NET Framework 4.7.2 and the current database schema.

## Evidence and regression coverage

| Workload | Before | After | Verification |
| --- | --- | --- | --- |
| Default database adapter, first configured endpoint succeeds | Three connection opens | One retained open connection | Recording connections; first, second, and fourth candidate success, empty list, all candidates fail |
| Catalog refresh with N cards | One catalog read plus two reads per card | One joined catalog read | 0, 1, and 25 card scenarios; live MariaDB join |
| Initial catalog selections | Selection events issue extra reads | One initial filtered read | STA form load with a recording adapter |
| Login screen and date formatter construction | Require a database connection | No database connection | Tests with no configured database |
| Authentication | Database and password verification block the UI | One background attempt; inputs restored on failure | STA tests with blocked authentication, duplicate submission, failed connection/retry, and disposal during a query |
| Shipping labels on customer, staff, and delivery order views | Fetch shipping data again for date calculations | Reuse the already fetched typed date | Date/culture matrix and direct review of the view call sites |
| Uploaded images | Source files remain locked; replaced bitmaps retained | Detached decoding and explicit bitmap ownership | Exclusive file access, replacement/clear, and control disposal tests |

For 25 catalog cards, the database-read count per refresh falls from 51 to 1. These counts describe the removed work; they are not an end-to-end latency benchmark.

Run the complete checks as described in [testing.md](testing.md). Database tests create and drop isolated schemas and do not change the app's seeded database. The parameterized scenarios use small, deterministic fixtures so failures identify a specific data case rather than depend on a timing threshold.

## Measure a development session

The application writes aggregate timings through the existing console/file logger under the `Performance` component:

- Authentication elapsed milliseconds, including connection setup when needed.
- Catalog query elapsed milliseconds and returned row count.
- Catalog control construction/layout elapsed milliseconds and rendered row count.

These messages contain no account IDs, passwords, SQL text, or search terms. Existing non-performance diagnostics remain separate. Render measurements cover synchronous construction/layout, not the final screen paint.

Exercise the same login outcome, catalog size, filters, and refresh sequence several times in a Release build. Keep the database, machine load, and configuration consistent when comparing runs. Record a cold run separately from repeated runs so connection/resource warm-up does not obscure changes.

Logs are written beneath the application's working directory. Summarize one session log or a directory from PowerShell:

```powershell
.\scripts\summarize-performance.ps1 -LogPath .\Logs\Log-2026-10-23-session.txt
.\scripts\summarize-performance.ps1 -LogPath .\Logs -OutputPath .\TestResults\performance-summary.json
```

Use an actual log filename for the first command and create the output directory if needed. The JSON contains sample count, minimum, nearest-rank p50/p95, and maximum milliseconds. Catalog results are grouped by operation and row count to keep different sizes separate. The script exports only parsed aggregate measurements. Small sample sets are useful for debugging, but their percentiles do not establish a stable performance improvement. Unit-test timings use recording adapters and do not measure production database latency.

Use the results to choose the next change, add a regression for the observed case, rerun the same workload, and compare the resulting call counts and latency distribution.
