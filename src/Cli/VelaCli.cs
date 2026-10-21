// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using Vela.UI.Cli.Commands;
using Vela.UI.Cli.Registry;

namespace Vela.UI.Cli;

/// <summary>
/// Parses command-line arguments and writes results to the given writers, so the
/// CLI can be tested without starting a process.
/// </summary>
internal static class VelaCli
{
    /// <summary>The command did what was asked.</summary>
    public const int Success = 0;

    /// <summary>The command couldn't run, for example because there's no Blazor project or no vela.json.</summary>
    public const int Failure = 1;

    /// <summary>The arguments weren't understood.</summary>
    public const int UsageError = 2;

    /// <summary>Files exist with different content, so nothing was written.</summary>
    public const int Conflict = 3;

    /// <summary>
    /// Runs the CLI and returns the process exit code.
    /// </summary>
    public static int Run(string[] args, TextWriter output, TextWriter error)
    {
        if (args.Length == 0 || args[0] is "-h" or "--help")
        {
            output.WriteLine(Usage());
            return Success;
        }

        if (args[0] == "--version")
        {
            output.WriteLine(GetVersion());
            return Success;
        }

        var command = args[0];
        if (command is not ("init" or "add"))
        {
            return Fail(error, $"Unknown command '{command}'.");
        }

        var directory = Directory.GetCurrentDirectory();
        var overwrite = false;
        var names = new List<string>();
        for (var i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-h" or "--help":
                    output.WriteLine(Usage());
                    return Success;
                case "--overwrite":
                    overwrite = true;
                    break;
                case "--cwd" when i + 1 < args.Length:
                    directory = Path.GetFullPath(args[++i]);
                    break;
                case "--cwd":
                    return Fail(error, "--cwd needs a folder.");
                case var option when option.StartsWith('-'):
                    return Fail(error, $"Unknown option '{option}'.");
                default:
                    names.Add(args[i]);
                    break;
            }
        }

        if (command == "init" && names.Count > 0)
        {
            return Fail(error, $"'vela init' doesn't take a component. Did you mean 'vela add {names[0]}'?");
        }

        if (command == "add" && names.Count == 0)
        {
            return Fail(error, "Name the components to add, for example 'vela add button card'.");
        }

        try
        {
            return command == "init"
                ? InitCommand.Run(directory, overwrite, output, error)
                : AddCommand.Run(directory, names, overwrite, output, error);
        }
        catch (CliException exception)
        {
            error.WriteLine(exception.Message);
            return Failure;
        }
        catch (IOException exception)
        {
            error.WriteLine($"Couldn't write the files: {exception.Message}");
            return Failure;
        }
        catch (UnauthorizedAccessException exception)
        {
            error.WriteLine($"Couldn't write the files: {exception.Message}");
            return Failure;
        }
    }

    /// <summary>
    /// The CLI's version, such as <c>0.1.0</c>. The bundled registry and the
    /// Vela.UI.Primitives package share it.
    /// </summary>
    public static string GetVersion()
    {
        var version = typeof(VelaCli).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "unknown";

        // The SDK appends "+<commit sha>" when building from git. Users only need the release version.
        var metadataStart = version.IndexOf('+');
        return metadataStart < 0 ? version : version[..metadataStart];
    }

    private static int Fail(TextWriter error, string message)
    {
        error.WriteLine($"{message} Run 'vela --help' for usage.");
        return UsageError;
    }

    private static string Usage()
    {
        // One line per few names, so the list fits a narrow terminal.
        var lines = new List<string> { "" };
        foreach (var name in BundledRegistry.Instance.Components.Select(component => component.Name))
        {
            if (lines[^1].Length + name.Length > 72)
            {
                lines.Add("");
            }

            lines[^1] += lines[^1].Length == 0 ? name : " " + name;
        }

        var components = string.Join(Environment.NewLine + "  ", lines);
        return $"""
            Usage: vela <command> [options]

            Commands:
              init                   Set up Vela UI in the Blazor project in this folder
              add <component>...     Copy components, and the components they use, into the project

            Options:
              --cwd <folder>         Run in this folder instead of the current one
              --overwrite            Replace files that differ, keeping the old ones in .vela/backups
              -h, --help             Show this help
              --version              Show the version

            Components:
              {components}

            Exit codes: 0 done, 1 couldn't run, 2 bad arguments, 3 files would be overwritten.
            """;
    }
}
