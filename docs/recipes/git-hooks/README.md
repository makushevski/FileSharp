# Git Hook Recipes

## Pre-commit: JSON and Large File Gate

Keep broken JSON and oversized files out of your repository by combining two reusable scripts inside a Git pre-commit hook.

### Scripts used

- [Validate JSON](../../scripts/tools/validate-json.md) — `scripts/tools/validate-json.cs`
- [Find Large Files](../../scripts/tools/find-large-files.md) — `scripts/tools/find-large-files.cs`

### Setup

1. **Create a hooks directory** inside your repo (e.g., `.githooks`) and tell Git to use it:

   ```bash
   mkdir -p .githooks
   git config core.hooksPath .githooks
   ```

2. **Add `.githooks/pre-commit.ps1`** (PowerShell example) that runs both scripts:

   ```powershell
   dotnet run scripts/tools/validate-json.cs -- --cwd $PWD .
   if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

   dotnet run scripts/tools/find-large-files.cs -- --path . --min-size 25MB --top 0
   if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
   ```

   - `--cwd $PWD` ensures the validator scans files relative to the repo root.
   - Adjust `--min-size` / `--top` so the size gate matches your policy. Make the script exit with a non-zero code when large files are found.

3. **(Optional) Provide a Bash equivalent** in `.githooks/pre-commit` for Linux/macOS contributors; call the same `dotnet run` commands.

4. **Grant execution permission** to the hook scripts (`chmod +x .githooks/pre-commit*` on Unix; set execution policy for PowerShell).

5. **Test the hook** by introducing a JSON syntax error and staging a large binary. `git commit` should be blocked with clear messages.

### Tips

- Share helper scripts (for example, `.githooks/common.ps1`) if multiple hooks reuse the same checks.
- Call `dotnet run ... -- --help` inside the hook to print friendly diagnostics when contributors forget required arguments.
- Document the workflow in your repo so teammates know how to reinstall hooks after cloning.
