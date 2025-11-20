# Contributing to FileSharp

This is the short guide—keep it handy while you build.

## Quick start
- Install .NET SDK 10+ (`dotnet --info`).
- Fork/clone, branch from `main`, and run any script (e.g. `dotnet run scripts/tools/hash-directory.cs -- ..`) to verify the toolchain.
- Make your change, run the script/tests, and jot down how you validated it.

## Essentials
- Open an issue when a feature or breaking change needs alignment; otherwise ship a focused branch/PR.
- Keep scripts single-file friendly: include the metadata header, stay ASCII, prefer BCL APIs, and make console output deterministic on Windows/Linux/macOS.
- Print clear usage/errors, fail with non-zero exit codes, and add dry-run flags when a script calls external services.
- Document every new or changed script in `docs/<category>/<script>.md`, mirror it in the README tables/recipe catalog, and mention prerequisites.
- Small PRs + clear testing notes get merged faster—describe what you ran and any manual steps.

## Before opening a PR
- [ ] Affected scripts build/run locally with realistic inputs.
- [ ] Metadata headers, docs (`docs/`, README tables, recipes) are updated.
- [ ] Error handling produces actionable messages.
- [ ] Tests or manual verification steps are captured in the PR body.

Thanks for keeping FileSharp sharp!


