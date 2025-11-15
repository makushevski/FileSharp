// # name: Validate JSON
// # category: tools
// # description: Проверяет JSON-файлы на синтаксические ошибки и показывает строку/столбец.
// # usage:
// #   dotnet run scripts/tools/validate-json.cs -- ./configs
// #   dotnet run scripts/tools/validate-json.cs -- ./config/appsettings.json

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

var parsedArgs = new List<string>();
var pattern = "*.json";
var recursive = true;
var exitCode = 0;

for (var i = 0; i < args.Length; i++)
{
    var arg = args[i];

    switch (arg)
    {
        case "--help":
        case "-h":
            PrintUsage();
            return;
        case "--pattern":
            if (i + 1 >= args.Length)
            {
                Console.WriteLine("[ERR] Missing value for --pattern.");
                PrintUsage();
                Environment.Exit(1);
                return;
            }

            pattern = args[++i];
            break;
        case "--no-recursive":
            recursive = false;
            break;
        default:
            parsedArgs.Add(arg);
            break;
    }
}

if (parsedArgs.Count == 0)
{
    PrintUsage();
    Environment.Exit(1);
    return;
}

var targets = ResolveTargets(parsedArgs, pattern, recursive);

if (targets.Count == 0)
{
    Console.WriteLine("[ERR] No files to validate. Adjust the path or pattern.");
    Environment.Exit(1);
    return;
}

var failures = 0;
var success = 0;

foreach (var file in targets.OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
{
    try
    {
        using var stream = File.OpenRead(file);
        JsonDocument.Parse(stream, new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Disallow,
            AllowTrailingCommas = false
        });

        Console.WriteLine($"[OK ] {file}");
        success++;
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"[ERR] {file} (line {ex.LineNumber}, byte {ex.BytePositionInLine}): {ex.Message}");
        failures++;
        exitCode = 2;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERR] {file}: {ex.Message}");
        failures++;
        exitCode = 2;
    }
}

Console.WriteLine();
Console.WriteLine($"Validated {success + failures} file(s). OK: {success}. Failed: {failures}.");
Environment.Exit(exitCode);

static List<string> ResolveTargets(IEnumerable<string> inputs, string pattern, bool recursive)
{
    var files = new List<string>();
    var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

    foreach (var input in inputs)
    {
        var fullPath = Path.GetFullPath(input);

        if (File.Exists(fullPath))
        {
            files.Add(fullPath);
            continue;
        }

        if (Directory.Exists(fullPath))
        {
            var matched = Directory
                .EnumerateFiles(fullPath, pattern, searchOption)
                .ToList();

            if (matched.Count == 0)
            {
                Console.WriteLine($"[WARN] No files matching '{pattern}' under {fullPath}.");
            }

            files.AddRange(matched);
            continue;
        }

        Console.WriteLine($"[ERR] Path not found: {input}");
    }

    return files
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();
}

static void PrintUsage()
{
    Console.WriteLine("Validate JSON files in a directory or a set of paths.");
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run scripts/tools/validate-json.cs -- <path> [more paths]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --pattern <glob>     File pattern for directories (default: *.json)");
    Console.WriteLine("  --no-recursive       Do not traverse subdirectories");
    Console.WriteLine("  -h, --help           Show this help message");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  dotnet run scripts/tools/validate-json.cs -- ./config");
    Console.WriteLine("  dotnet run scripts/tools/validate-json.cs -- ./config/appsettings.json ./extra/settings.json");
}
