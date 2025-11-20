# FileSharp ⚙️

![License](https://img.shields.io/github/license/makushevski/FileSharp)
![Stars](https://img.shields.io/github/stars/makushevski/FileSharp?style=social)

FileSharp is a toolkit of single-file C# utilities for DevOps, CI/CD, operations, and database teams. Every tool is a regular C# file that can be executed with `dotnet run`, so you can keep automation scripts versioned together with infrastructure definitions.

Think of it as a friendly Swiss Army knife for ops chores 🧰—grab a script, run it, and move on with your day. If you need to change something, you can fix it fast.

## Highlights ✨

- **📦 File-based scripts** - no custom runners; each `.cs` file is a self-contained tool.
- **🆕 Modern C#** - top-level statements, nullable annotations, and .NET 10 runtime support.
- **🧭 Deterministic behavior** - scripts define metadata in comments so they can be indexed automatically.
- **🌍 Works anywhere** - Linux, macOS, and Windows as long as the .NET SDK is installed.
- **📚 Documentation-first** - every script is paired with a short Markdown page inside `docs/`.
- **🧩 Recipes** - you can find some hands on recipes for some scripts in [this table](docs/recipes/README.md).

## Repository Layout 🗂️

```
scripts/
  ci/                # CI/CD helpers (version bump, release checks, etc.)
  tools/             # Day-to-day utilities (hashing, renaming, cleanup)
docs/
  tools/             # Reference for each script
LICENSE
README.md
CONTRIBUTING.md
```

Only a subset of scripts are currently published; new utilities follow the same layout.

## Requirements 🧪

1. .NET SDK 10.0 or newer (file-based programs require .NET 10 features).
2. PowerShell, Bash, or any shell capable of launching `dotnet run`.
3. (Optional) Access to your infrastructure if a script talks to external systems (Kubernetes, Git, DB, etc.).

Verify your setup:

```bash
dotnet --info
```

## Usage 🚀

1. Locate the script you need (see the catalog below).
2. Run it with `dotnet run <path-to-script> -- <arguments>`.

Example - hashing a directory:

```bash
dotnet run scripts/tools/hash-directory.cs -- ./infra/helm
```

Every script prints a short help section when executed without arguments. Follow the metadata header to understand what a tool does:

```csharp
// # name: Hash Directory
// # category: tools
// # description: Produce per-file SHA256 checksums and a combined digest.
```

The metadata keeps the catalog up to date and powers future automation (indexing, docs, packaging).

## Script Catalog

The detailed catalog now lives in [docs/scripts/README.md](docs/scripts/README.md). Browse that page for per-category tables, command summaries, and links to every script doc.


## Recipes 🧩

Scenario guides show how to mix multiple scripts to solve workflow problems (Git hooks, CI stages, operations runbooks).

- **🧩 Recipes** - you can find some hands on recipes for some scripts in [this table](docs/recipes/README.md).

## Creating a New Script 🔧

1. Copy an existing file (e.g., `hash-directory.cs`) into the right category.
2. Update the metadata block at the top (`name`, `category`, `description`).
3. Implement the logic using top-level statements. Prefer deterministic console output.
4. Create `docs/scripts/<category>/<script-name>.md` with usage examples, parameters, and caveats.
5. Update `docs/scripts/README.md` (the catalog) and, if needed, add automation examples to `docs/`.

### Script Header Format 📝

Every script starts with the same metadata header, including at least one usage example:

```csharp
// # name: Hash Directory
// # category: tools
// # description: Produce per-file SHA256 checksums and a combined digest.
// # usage:
// #   dotnet run scripts/tools/hash-directory.cs -- .
// #   dotnet run scripts/tools/hash-directory.cs -- C:\Projects\MyApp
```

Add additional `// #   ...` usage lines if a script has more than one common invocation. Keep all content in ASCII where possible so documentation renders correctly on any locale.

## Documentation 📘

- `docs/` contains human-friendly walkthroughs and usage notes.
- `docs/scripts/<category>/` stores per-script references; feel free to add more directories such as `docs/scripts/ci/`.
- Keep prose concise and link back to the command examples shown above.

## Contributing 🤝

We welcome pull requests! Please read `CONTRIBUTING.md` for details on how to set up your environment, follow the coding guidelines, and run verification scripts before opening a PR.

## License 📄

FileSharp is distributed under the terms of the MIT License. See `LICENSE` for full text.

Happy scripting and automate boldly! 😄



