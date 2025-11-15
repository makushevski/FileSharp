# Validate JSON

Validate JSON files recursively (or per file) and report syntax errors with line/column info.

## Requirements

- .NET SDK 10.0+
- Access to the files or directories you want to inspect

## Usage

```bash
dotnet run scripts/tools/validate-json.cs -- ./config

dotnet run scripts/tools/validate-json.cs -- ./config/appsettings.json ./extra/settings.json
```

## Options

- `--pattern <glob>` - limit files inside directories (default `*.json`).
- `--no-recursive` - scan only the specified directory, without subfolders.
- `-h`, `--help` - print usage instructions.

## Output

Each processed file prints `[OK ] path` when valid. Invalid files print `[ERR]` with the JSON parser message plus the line and byte numbers.

```
[OK ] C:/repo/config/appsettings.json
[ERR] C:/repo/config/broken.json (line 14, byte 9): '}' expected.
Validated 2 file(s). OK: 1. Failed: 1.
```

## Exit Codes

- `0` - every file is valid JSON.
- `1` - invalid arguments or no files matched the inputs.
- `2` - at least one file failed validation (the error output is printed for each file).

## Notes

- Uses `JsonDocument.Parse` with strict settings (`AllowTrailingCommas=false`, comments disallowed).
- Directory traversal is case-insensitive and deduplicated so the same file is not validated twice.
- Combine it with CI by pointing the script at the folder that stores configuration templates.
