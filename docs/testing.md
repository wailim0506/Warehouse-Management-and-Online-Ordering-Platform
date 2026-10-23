# Testing

The application remains a Windows Forms application targeting .NET Framework 4.7.2. Install Visual Studio or Build Tools with the .NET desktop build tools and the 4.7.2 targeting pack.

From PowerShell at the repository root:

```powershell
.\scripts\test.ps1 -Configuration Release
.\scripts\test.ps1 -Configuration Debug
```

The script restores packages, builds the solution, executes xUnit, and checks the generated TRX report. Test failures, failed stages, and zero discovered tests produce an error. Reports are saved in dated directories under `TestResults` by default.

Ordinary controller and form tests use recording adapters and do not connect to MySQL. The form test constructs and disposes the login form on an STA thread without loading live services. Logging tests use isolated application domains and temporary directories.

## Database integration tests

Database integration tests are skipped unless `WAREHOUSE_TEST_CONNECTION_STRING` is set. They cover adapter commit/rollback and command paths, nested-transaction rejection, simultaneous shipping requests, simultaneous order additions, replayed restocks, and rollback after injected shipping/restock failures.

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

The supplied account needs privileges to create/drop databases and create triggers. Each test creates an InnoDB schema named `warehouse_test_<guid>`, then removes it. The database named in the connection string is never selected or modified. The test fixture imposes short connection/command timeouts and disables pooling so its connections close before schema cleanup.

The inventory fixture includes the columns and keys used by the tested operations; it does not replace validation of the complete seed, schema relationships, or interactive application workflows.

## Verified results

On 2026-10-23, the native script restored and built both Debug and Release, then passed all 50 tests in each configuration with no skipped tests. Eight tests in each run exercised a live MariaDB 10.5.29 instance using Docker in WSL. Those eight integration tests also passed against a separate MariaDB 11.4.5 instance.

The existing Compose seed imported successfully with 33 tables, and phpMyAdmin responded with HTTP 200. The wiki built successfully with 20 pages. Interactive desktop workflows were not exercised, and the existing compiler warnings remain. VM workflow changes are outside this refinement's scope.
