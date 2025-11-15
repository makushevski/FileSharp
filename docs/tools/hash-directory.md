# Hash Directory

Produce deterministic SHA256 checksums for every file under the target path and emit a combined digest that can be used to detect drift.

## Requirements

- .NET SDK 10.0+
- Read access to the target directory

## Usage

```bash
dotnet run scripts/tools/hash-directory.cs -- <path>
```

Examples:

```bash
# Hash the current folder
dotnet run scripts/tools/hash-directory.cs -- .

# Hash a Helm chart directory
dotnet run scripts/tools/hash-directory.cs -- ./infra/helm
```

## Output

For each file the script prints `SHA256  relative/path`. After processing all entries it prints an overall digest that is derived from the ordered list of files and hashes.

```
50a4c7...  chart/values.yaml
b1dd2a...  chart/templates/deployment.yaml
========================================
Combined digest (SHA256): 6f954d...
========================================
```

## Exit Codes

- `0` - success.
- `1` - invalid arguments or the path does not exist.
- `2` - at least one file could not be read (an error message is printed, processing continues).

## Notes

- Files are enumerated using an ordinal, case-insensitive order to keep the output stable across platforms.
- Binary and text files are treated the same; if you need to ignore a subset of files, filter them before running the script.
- Use the combined digest inside CI pipelines to detect configuration drift between deployments.

