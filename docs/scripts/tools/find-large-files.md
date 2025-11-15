# Find Large Files

Scan directories for files above a threshold to catch oversized artifacts.

## Usage

```bash
# list top 20 files bigger than 20 MB
dotnet run scripts/tools/find-large-files.cs -- --path . --min-size 20MB --top 20

# scan a single directory without recursion
dotnet run scripts/tools/find-large-files.cs -- --path ./artifacts --no-recursive --min-size 50MB
```

## When to use

- Prevent accidental commits of huge binaries or build artifacts.
- Identify cache/build output folders that need pruning before releases.
- Enforce repository policies (e.g., max file size) in CI or hooks.

## Script

- [`scripts/tools/find-large-files.cs`](../../../scripts/tools/find-large-files.cs)

[⬅ Back to the cookbook index](../../README.md)
