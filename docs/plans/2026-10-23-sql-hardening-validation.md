# SQL hardening and validation implementation plan

**Goal:** Make the existing SQL-hardening branch reliably testable and fix confirmed controller, inventory, and logging failures. VM workflow changes are excluded at the user's request.

**Architecture:** Preserve the .NET Framework 4.7.2 Windows Forms application and MySQL schema. Use the existing `IDatabase` boundary for isolated controller tests, and add an explicit transaction boundary for multi-step stock operations. Keep dependency and workflow changes small and compatible with the existing solution.

**Baseline:** Branch `feature/sql-hardening-vm-workflow`, commit `2b5ad49`. Windows Release build succeeds with existing compiler warnings. Ten non-UI tests execute with an unreachable, isolated output-only database configuration: one passes, nine fail during construction because the database fakes inherit the live adapter. The wiki builds successfully. Native Docker is unavailable; Docker in WSL was subsequently found and used for live database validation.

## 1. Isolate controller tests

- Change tested controllers and validators to accept `IDatabase` while preserving existing production construction paths.
- Replace live adapter subclasses with recording in-memory implementations; fail on unconfigured database operations.
- Cover login success/failure, account state, recovery, and favourites with real controller behavior and bound SQL parameters.
- Ensure pure validation never opens a connection. Remove production connection strings from the unit-test configuration.
- Replace the open-ended login window test with a deterministic STA construction/disposal check that does not load live services.
- Enable generated binding redirects and run every discovered xUnit test with no live MySQL server.

## 2. Make stock changes atomic

- Add `ExecuteInTransaction(Action)` to `IDatabase` and the MySQL adapter. Attach every command to the active transaction and roll back failed operations.
- Reproduce duplicate deductions/restocks and invalid or excessive quantities with controller regressions before fixing them.
- Lock and validate source order/reorder state before mutations, reject invalid quantities, and roll back all writes together on failure.
- Cover repeat calls, insufficient inventory, and rollback paths using isolated database test doubles.
- Make unsupported public sort values fail consistently without interpolating arbitrary SQL.

## 3. Repair logging and dependency consistency

- Reproduce logging before explicit initialization and logging to an unwritable location.
- Make diagnostics safe to initialize and use concurrently, and keep file failures from breaking application operations.
- Remove passwords from authentication diagnostics.
- Align the application and controller `System.Text.Json` package declarations, references, and redirects to 8.0.6, then repeat package restore and check its warnings.

## 4. Validate native builds and live database behavior

- Add repeatable Windows restore/build/test commands, with nonzero exit codes on failed stages and zero discovered tests treated as a failure.
- Run all controller tests after compilation and preserve test result artifacts.
- Make database integration tests opt-in using an environment connection string, with a fresh disposable schema per test.
- Exercise commit, rollback, reader/command paths, concurrent shipping and order additions, and restock replay against real MariaDB instances.
- Document actual prerequisites, commands, and limitations in the README.

## 5. Final verification and review

- Restore packages and build the solution in Debug and Release with Visual Studio MSBuild.
- Execute all isolated xUnit tests and live database integration tests.
- Validate Compose configuration and build the wiki with its lockfile.
- Review the final diff for accidental data, build artifacts, credentials, unrelated changes, and remaining material limitations.
- Obtain an independent review, then commit and update the requested feature branch without modifying `master`.

See [Testing](../testing.md#verified-results) for the completed validation results and remaining limitations.
