# Contributing to FileSharp

Thanks for helping make FileSharp a reliable toolkit for DevOps automation. Please read this guide before opening an issue or pull request.

## Quick Start

1. Install the .NET SDK 10.0+ and ensure `dotnet --info` works.
2. Clone the repository and create a feature branch: `git checkout -b feature/my-script`.
3. Run an existing script to verify your environment, e.g. `dotnet run scripts/tools/hash-directory.cs -- ..`.
4. Make your changes, add tests or usage samples if applicable, and run the script locally.

## Development Workflow

1. **Discuss first (optional but recommended).** Open a GitHub issue describing the problem or the tool you plan to add.
2. **Fork & branch.** Keep commits focused; prefer small, reviewable pull requests.
3. **Follow the style guide.** Use top-level statements, meaningful variable names, and deterministic console output.
4. **Validate.** Run the script(s) you changed with representative inputs. If a script touches infrastructure, provide a dry-run option when possible.
5. **Document.** Update `README.md`, the script catalog tables, and create/update `docs/<category>/<script>.md` so users understand your changes.
6. **Submit a PR.** Fill in the template, link related issues, and describe how you tested the change. Maintainers will review and may request adjustments.

## Coding Guidelines

- **Metadata header.** Every script starts with `// # name`, `// # category`, `// # description`, and a `// # usage:` block that lists at least one invocation (each prefixed with `// #   ...`). Keep text short, actionable, and ASCII-friendly so it renders on any locale.
- **Input handling.** Show usage instructions when arguments are missing. Provide clear error messages with exit codes > 0 when failing.
- **Dependencies.** Prefer BCL APIs; avoid pulling NuGet packages unless strictly required. If you add a package, document why and update restore instructions.
- **Formatting.** Use spaces, `var` when the type is obvious, and guard external calls with retries or timeouts if appropriate.
- **Logging.** Write to stdout for normal information and prefix non-fatal warnings with `[WARN]`. Use `[ERR]` for recoverable problems.
- **Cross-platform.** File paths, environment variables, and shell commands must work on Windows, Linux, and macOS.

## Documentation Standards

- Place script docs under `docs/<category>/<script-name>.md`.
- Include a brief summary, prerequisites, command examples, parameters, and failure modes.
- Keep prose in English and ASCII to avoid encoding issues.
- Update tables/lists in `README.md` so the script catalog matches the new doc.

## Checklist Before Opening a Pull Request

- [ ] All scripts compile and run locally with `.NET 10`.
- [ ] Added/updated docs in `docs/` and cross-linked from the README.
- [ ] Metadata headers are present and accurate.
- [ ] Errors are handled gracefully, with actionable messages.
- [ ] Tests or manual verification steps are described in the PR body.
- [ ] The PR title/description explain the problem and the proposed solution.

By following these steps you keep FileSharp predictable and easy to adopt for other teams. Thank you!
