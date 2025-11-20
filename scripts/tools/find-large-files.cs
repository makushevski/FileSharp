// # name: Find Large Files
// # category: tools
// # description: List the largest files under a path so you can catch oversized artifacts before they land in git or CI output.
// # usage:
// #   dotnet run scripts/tools/find-large-files.cs -- --path . --min-size 25MB --top 20
// #   dotnet run scripts/tools/find-large-files.cs -- --path ./artifacts --no-recursive --min-size 50MB

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

string? targetPath = null;
var recursive = true;
var top = 20;
long minSizeBytes = 20 * 1024 * 1024;

for (var i = 0; i < args.Length; i++)
{
    var arg = args[i];

    switch (arg)
    {
        case "--path":
            if (!TryReadValue(args, ref i, out targetPath))
            {
                Console.WriteLine("[ERR] Missing value for --path.");
                return;
            }

            break;
        case "--min-size":
            if (!TryReadValue(args, ref i, out var sizeText) || !TryParseSize(sizeText!, out minSizeBytes))
            {
                Console.WriteLine("[ERR] Provide --min-size like 10MB, 512KB, 1GB.");
                return;
            }

            if (minSizeBytes <= 0)
            {
                Console.WriteLine("[ERR] --min-size must be positive.");
                return;
            }

            break;
        case "--top":
            if (!TryReadValue(args, ref i, out var topText) || !int.TryParse(topText, NumberStyles.Integer, CultureInfo.InvariantCulture, out top) || top < 0)
            {
                Console.WriteLine("[ERR] --top must be a non-negative integer.");
                return;
            }

            break;
        case "--no-recursive":
            recursive = false;
            break;
        case "--help":
        case "-h":
            PrintUsage();
            return;
        default:
            Console.WriteLine($"[WARN] Ignored unknown argument: {arg}");
            break;
    }
}

if (string.IsNullOrWhiteSpace(targetPath))
{
    PrintUsage();
    Environment.Exit(1);
    return;
}

var rootPath = Path.GetFullPath(targetPath);

if (!Directory.Exists(rootPath))
{
    Console.WriteLine($"[ERR] Directory not found: {rootPath}");
    Environment.Exit(1);
    return;
}

Console.WriteLine($"Scanning: {rootPath}");
Console.WriteLine($"Recursive: {recursive}");
Console.WriteLine($"Minimum size: {FormatSize(minSizeBytes)}");
Console.WriteLine($"Top count: {(top == 0 ? "all matches" : top.ToString(CultureInfo.InvariantCulture))}");
Console.WriteLine();

var matches = new List<FileEntry>();

foreach (var file in EnumerateFilesSafe(rootPath, recursive))
{
    try
    {
        var info = new FileInfo(file);
        if (info.Exists && info.Length >= minSizeBytes)
        {
            matches.Add(new FileEntry(file, info.Length));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[WARN] Skipping {file}: {ex.Message}");
    }
}

if (matches.Count == 0)
{
    Console.WriteLine("No files matched the specified size threshold.");
    return;
}

var ordered = matches
    .OrderByDescending(f => f.Size)
    .ThenBy(f => f.Path, StringComparer.OrdinalIgnoreCase)
    .ToList();

var limited = top > 0 ? ordered.Take(top).ToList() : ordered;

Console.WriteLine($"Found {ordered.Count} file(s) >= {FormatSize(minSizeBytes)}.");
Console.WriteLine();
Console.WriteLine($"{FormatColumn("Size", 12)}  Path");
Console.WriteLine(new string('-', 12) + "  ----");

foreach (var entry in limited)
{
    var relative = Path.GetRelativePath(rootPath, entry.Path);
    Console.WriteLine($"{FormatColumn(FormatSize(entry.Size), 12)}  {relative}");
}

if (top > 0 && ordered.Count > top)
{
    Console.WriteLine();
    Console.WriteLine($"...and {ordered.Count - top} more. Increase --top or set it to 0 to see all matches.");
}

static IEnumerable<string> EnumerateFilesSafe(string root, bool recursive)
{
    var stack = new Stack<string>();
    stack.Push(root);

    while (stack.Count > 0)
    {
        var current = stack.Pop();
        string[] files = Array.Empty<string>();

        try
        {
            files = Directory.GetFiles(current);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN] Cannot list files in {current}: {ex.Message}");
        }

        foreach (var file in files)
        {
            yield return file;
        }

        if (!recursive)
        {
            continue;
        }

        string[] directories = Array.Empty<string>();
        try
        {
            directories = Directory.GetDirectories(current);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARN] Cannot list directories in {current}: {ex.Message}");
        }

        foreach (var directory in directories)
        {
            stack.Push(directory);
        }
    }
}

static string FormatSize(long bytes)
{
    string[] units = { "B", "KB", "MB", "GB", "TB" };
    double value = bytes;
    var unitIndex = 0;

    while (value >= 1024 && unitIndex < units.Length - 1)
    {
        value /= 1024;
        unitIndex++;
    }

    return value >= 10 || unitIndex == 0
        ? $"{value:0} {units[unitIndex]}"
        : $"{value:0.0} {units[unitIndex]}";
}

static string FormatColumn(string value, int width)
{
    if (value.Length >= width)
    {
        return value;
    }

    return value.PadLeft(width);
}

static bool TryParseSize(string input, out long bytes)
{
    bytes = 0;
    if (string.IsNullOrWhiteSpace(input))
    {
        return false;
    }

    input = input.Trim();
    var index = 0;
    while (index < input.Length && (char.IsDigit(input[index]) || input[index] == '.' || input[index] == ','))
    {
        index++;
    }

    if (index == 0)
    {
        return false;
    }

    var numberPart = input[..index].Replace(',', '.');
    if (!double.TryParse(numberPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) || value <= 0)
    {
        return false;
    }

    var unitPart = input[index..].Trim().ToUpperInvariant();
    var multiplier = unitPart switch
    {
        "" or "B" => 1d,
        "KB" or "K" => 1024d,
        "MB" or "M" => 1024d * 1024,
        "GB" or "G" => Math.Pow(1024, 3),
        "TB" or "T" => Math.Pow(1024, 4),
        _ => -1d
    };

    if (multiplier < 0)
    {
        return false;
    }

    bytes = (long)(value * multiplier);
    return true;
}

static bool TryReadValue(string[] arguments, ref int index, out string? value)
{
    if (index + 1 >= arguments.Length)
    {
        value = null;
        return false;
    }

    value = arguments[++index];
    return true;
}

static void PrintUsage()
{
    Console.WriteLine("Find the largest files under a directory.");
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run scripts/tools/find-large-files.cs -- --path <directory> [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --path <dir>         Root directory to scan (required).");
    Console.WriteLine("  --min-size <value>   Minimum size threshold like 10MB, 512KB. Default: 20MB.");
    Console.WriteLine("  --top <n>            Number of entries to display. Use 0 to list all matches. Default: 20.");
    Console.WriteLine("  --no-recursive       Do not scan subdirectories.");
    Console.WriteLine("  -h, --help           Show this message.");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  dotnet run scripts/tools/find-large-files.cs -- --path . --min-size 25MB --top 20");
    Console.WriteLine("  dotnet run scripts/tools/find-large-files.cs -- --path ./artifacts --no-recursive --min-size 50MB");
}

record FileEntry(string Path, long Size);
