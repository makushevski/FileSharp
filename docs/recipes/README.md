# Recipe Catalog

Scenario guides show how to chain multiple FileSharp scripts into a cohesive workflow. Use the table below to explore existing recipes and spot reusable building blocks for your automation playbook.

| Recipe | Workflow Focus | Scripts Involved | Highlights |
| ------ | -------------- | ---------------- | ---------- |
| [Git Hooks: Pre-commit JSON + Large File Gate](git-hooks/README.md#pre-commit-json-and-large-file-gate) | Keep malformed JSON and oversized binaries from entering the repo during `git commit`. | [`validate-json`](../scripts/tools/validate-json.md), [`find-large-files`](../scripts/tools/find-large-files.md) | Runs in PowerShell or Bash, fails fast with clear errors, and scales to any repo size. |

_Add a new row whenever you publish a scenario so contributors can discover it quickly._
