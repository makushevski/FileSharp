// # name: Hash Directory
// # category: tools
// # description: Produce deterministic SHA256 checksums for every file under the target path and emit a combined digest
// # usage:
// #   dotnet run scripts/tools/hash-directory.cs -- .
// #   dotnet run scripts/tools/hash-directory.cs -- C:\Projects\MyApp

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

if (args.Length == 0)
{
    PrintUsage();
    Environment.Exit(1);
}

var rootPath = Path.GetFullPath(args[0]);

if (!Directory.Exists(rootPath))
{
    Console.WriteLine($"Directory does not exist: {rootPath}");
    Environment.Exit(1);
}

Console.WriteLine($"Target directory: {rootPath}");
Console.WriteLine();

var files = Directory
    .EnumerateFiles(rootPath, "*", SearchOption.AllDirectories)
    .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
    .ToList();

if (files.Count == 0)
{
    Console.WriteLine("No files found under the provided path.");
    Environment.Exit(0);
}

using var overallSha = SHA256.Create();
var overallBuilder = new StringBuilder();
var exitCode = 0;

foreach (var file in files)
{
    var relative = Path.GetRelativePath(rootPath, file);

    try
    {
        using var stream = File.OpenRead(file);
        using var fileSha = SHA256.Create();
        var hashBytes = fileSha.ComputeHash(stream);
        var hash = ToHex(hashBytes);

        Console.WriteLine($"{hash}  {relative}");

        overallBuilder.Append(relative);
        overallBuilder.Append('\n');
        overallBuilder.Append(hash);
        overallBuilder.Append('\n');
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERR] Could not hash {relative}: {ex.Message}");
        exitCode = 2;
    }
}

var overallBytes = Encoding.UTF8.GetBytes(overallBuilder.ToString());
var overallHashBytes = overallSha.ComputeHash(overallBytes);
var overallHash = ToHex(overallHashBytes);

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine($"Combined digest (SHA256): {overallHash}");
Console.WriteLine("========================================");

Environment.Exit(exitCode);

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run scripts/tools/hash-directory.cs -- <path>");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  dotnet run scripts/tools/hash-directory.cs -- .");
    Console.WriteLine("  dotnet run scripts/tools/hash-directory.cs -- C:\\Projects\\MyApp");
}

static string ToHex(byte[] bytes)
{
    var builder = new StringBuilder(bytes.Length * 2);

    foreach (var b in bytes)
    {
        builder.Append(b.ToString("x2"));
    }

    return builder.ToString();
}

