# FileSharp

FileSharp is a toolkit of single-file C# utilities for DevOps, CI/CD, operations, and database teams. Every tool is a regular C# file that can be executed with `dotnet run`, so you can keep automation scripts versioned together with infrastructure definitions.

## Highlights

- **File-based scripts** - no custom runners; each `.cs` file is a self-contained tool.
- **Modern C#** - top-level statements, nullable annotations, and .NET 10 runtime support.
- **Deterministic behavior** - scripts define metadata in comments so they can be indexed automatically.
- **Works anywhere** - Linux, macOS, and Windows as long as the .NET SDK is installed.
- **Documentation-first** - every script is paired with a short Markdown page inside `docs/`.

## Repository Layout

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

## Requirements

1. .NET SDK 10.0 or newer (file-based programs require .NET 10 features).
2. PowerShell, Bash, or any shell capable of launching `dotnet run`.
3. (Optional) Access to your infrastructure if a script talks to external systems (Kubernetes, Git, DB, etc.).

Verify your setup:

```bash
dotnet --info
```

## Usage

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

### Tools

| Script | Summary |
| ------ | ------- |
| [`hash-directory`](docs/tools/hash-directory.md) | Calculates SHA256 for every file in a directory and prints an overall digest. |

### CI/CD

_(Add entries as they are published.)_

### DevOps / Infrastructure

_(Add entries as they are published.)_

### Database

_(Add entries as they are published.)_

More scripts are coming soon. When you add a new tool, describe it under the appropriate category table, link to its doc page, and add the `.cs` location in the summary if helpful.

## Creating a New Script

1. Copy an existing file (e.g., `hash-directory.cs`) into the right category.
2. Update the metadata block at the top (`name`, `category`, `description`).
3. Implement the logic using top-level statements. Prefer deterministic console output.
4. Create `docs/<category>/<script-name>.md` with usage examples, parameters, and caveats.
5. Update the README catalog and, if needed, add automation examples to `docs/`.

### Script Header Format

Every script starts with the same metadata header, including at least one usage example:

```csharp
// # name: Hash Directory
// # category: tools
// # description: Считает SHA256-хеш для всех файлов в папке и общий хеш по содержимому/именам.
// # usage:
// #   dotnet run scripts/tools/hash-directory.cs -- .
// #   dotnet run scripts/tools/hash-directory.cs -- C:\Projects\MyApp
```

Add additional `// #   ...` usage lines if a script has more than one common invocation. Keep all content in ASCII where possible so documentation renders correctly on any locale.

## Documentation

- `docs/` contains human-friendly walkthroughs and usage notes.
- `docs/tools/` stores per-script references; feel free to add more directories such as `docs/ci/`.
- Keep prose concise and link back to the command examples shown above.

## Contributing

We welcome pull requests! Please read `CONTRIBUTE.md` for details on how to set up your environment, follow the coding guidelines, and run verification scripts before opening a PR.

## License

FileSharp is distributed under the terms of the MIT License. See `LICENSE` for full text.

