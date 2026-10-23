# WinForms performance implementation plan

**Goal:** Remove confirmed startup, catalog, and image-lifetime bottlenecks while preserving .NET Framework 4.7.2, WinForms, and the current workflows.

**Architecture:** Keep database connections local to their owner and use the existing IDatabase boundary for deterministic regressions. Populate catalog cards from one joined read rather than querying per card. Move login work off the UI thread and keep form state changes on that thread. Track uploaded bitmap ownership explicitly so shared resource icons remain usable.

**Baseline:** Commit `60ffb4a`. All 50 Debug tests pass, including eight database integration tests against MariaDB in WSL. The running application remains in the original checkout; this work uses a separate worktree.

## 1. Connection acquisition

- Preserve configured fallback order and explicit connection-string behavior.
- Open each candidate once, retain the successful open connection, and dispose unsuccessful candidates.
- Test fallback attempts, empty/all-failed candidates, and ownership with provider-independent DbConnection fakes.
- Continue exercising actual transactions through the existing database integration suite.

## 2. Catalog rendering

- Join product price and category name into the catalog query without changing existing positional spare-part fields.
- Render from the fetched table and suppress event-driven duplicate initial queries.
- Dispose removed dynamic cards, suspend bulk layout, and preserve shared resource images.
- Add STA regressions for rendering with no per-card queries and disposal after repeated refreshes.

## 3. Uploaded images

- Introduce a detached image loader and explicitly tracked replacement/disposal.
- Dispose image file handles immediately after decoding, and dispose form-owned bitmaps when replaced, cleared, or closed.
- Dispose upload dialogs and apply the helper to existing upload/profile flows.
- Verify exclusive file access after loading and disposal of owned images without disposing unrelated images.

## 4. Startup and login responsiveness

- Make date formatting and loading the login screen independent of database availability.
- Open database services on demand during authentication and run blocking connection/hash/query work outside the UI thread.
- Prevent overlapping attempts, restore controls after failures, and release owned database services on form closure.
- Add deterministic STA regressions that simulate blocked authentication and verify UI responsiveness and duplicate-attempt protection.

## 5. Verification and integration

- Capture failing regressions before fixes, then run focused tests after each change.
- Build and run the complete suite in Debug and Release with live MariaDB in WSL.
- Compare catalog query counts and connection opening counts rather than relying on noisy wall-clock thresholds.
- Smoke-test the revised login window, review resource ownership and thread boundaries independently, and update the requested feature branch.
- Leave the user's existing application process and database running.

## 6. Data-driven development

- Parameterize connection fallback, catalog size, input validation, and date/culture scenarios.
- Record aggregate authentication, catalog query, and catalog rendering elapsed times without user inputs.
- Summarize repeated measurements as JSON grouped by operation and row count, with sample count and p50/p95 latency.
- Document the measured call-count reductions and how to reproduce the regression and timing checks; avoid asserting unmeasured end-to-end speedups.
