# Testing

The application remains a Windows Forms application targeting .NET Framework 4.7.2. Install Visual Studio or Build Tools with the .NET desktop build tools and the 4.7.2 targeting pack.

From PowerShell at the repository root:

```powershell
.\scripts\test.ps1 -Configuration Release
.\scripts\test.ps1 -Configuration Debug
```

The script restores packages, builds the solution, executes xUnit, and checks the generated TRX report. Test failures, failed stages, and zero discovered tests produce an error. Reports are saved in dated directories under `TestResults` by default.

Ordinary controller and form tests use recording adapters and do not connect to MySQL. WinForms tests run on STA threads with bounded waits and cover offline login loading, background authentication, retry/disposal, catalog rendering, and image ownership. Logging tests use isolated application domains and temporary directories. See [performance.md](performance.md) for the data-driven scenarios and aggregate timing workflow.

## Database integration tests

Database integration tests are skipped unless `WAREHOUSE_TEST_CONNECTION_STRING` is set. They cover adapter commit/rollback and command paths, nested-transaction rejection, simultaneous shipping requests, simultaneous order additions, replayed restocks, rollback after injected shipping/restock failures, and the joined catalog query.

Start a disposable MySQL/MariaDB development instance. For the existing Compose setup, run this from a WSL shell at the repository root, keeping the WSL session open while the Windows tests run:

```bash
docker compose -f docker/Docker-compose.yml up -d
```

Set the connection string in the Windows PowerShell session running the tests. This example uses the Compose development defaults; adjust the port and credentials for your instance:

```powershell
$env:WAREHOUSE_TEST_CONNECTION_STRING = 'Server=127.0.0.1;Port=3306;User ID=root;Password=rootpassword;SslMode=None'
try {
    .\scripts\test.ps1 -Configuration Release
} finally {
    Remove-Item Env:WAREHOUSE_TEST_CONNECTION_STRING
}
```

The supplied account needs privileges to create/drop databases and create triggers. Each test creates an isolated schema named `warehouse_test_<guid>` or `catalog_test_<guid>`, then removes it. The database named in the connection string is never selected or modified. The test fixtures impose short connection/command timeouts and disable pooling so their connections close before schema cleanup.

The inventory fixture includes the columns and keys used by the tested operations; it does not replace validation of the complete seed, schema relationships, or interactive application workflows.

## Verified results

On 2026-10-23, the performance refinement restored and built both Debug and Release, then passed all 81 tests in each configuration with no skipped tests. Nine tests in each run exercised live MariaDB using Docker in WSL, including the new catalog join. The performance-summary script was also checked with single/zero-valued measurements, multiple row-count groups, percentile examples, and an empty input; its normal path ran under Windows PowerShell 5.1 as well as PowerShell 7.

The Release executable was launched and its login window, enabled login action, user-ID label, and UI responsiveness were verified through Windows UI Automation. Full interactive desktop workflows were not exercised, and existing compiler warnings remain. VM workflow changes are outside this refinement's scope.

The preceding SQL-hardening baseline passed all 50 tests in both configurations, including eight integration tests on MariaDB 10.5.29 and separately MariaDB 11.4.5. That baseline also verified the Compose seed with 33 tables, phpMyAdmin HTTP 200, and a 20-page wiki build; those checks were not repeated by this performance-only refinement.
