// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;

namespace Vela.UI.Cli;

/// <summary>
/// Parses command-line arguments and writes results to the given writers, so the
/// CLI can be tested without starting a process.
/// </summary>
internal static class VelaCli
{
    public const int Success = 0;
    public const int UsageError = 2;

    private const string Usage = """
        Usage: vela [options]

        Options:
          -h, --help    Show this help
          --version     Show the version
        """;

    /// <summary>
    /// Runs the CLI and returns the process exit code: 0 on success, 2 for
    /// unrecognized arguments.
    /// </summary>
    public static int Run(string[] args, TextWriter output, TextWriter error)
    {
        if (args.Length == 0 || args[0] is "-h" or "--help")
        {
            output.WriteLine(Usage);
            return Success;
        }

        if (args[0] == "--version")
        {
            output.WriteLine(GetVersion());
            return Success;
        }

        error.WriteLine($"Unknown argument '{args[0]}'. Run 'vela --help' for usage.");
        return UsageError;
    }

    private static string GetVersion()
    {
        var version = typeof(VelaCli).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "unknown";

        // The SDK appends "+<commit sha>" when building from git. Users only need the release version.
        var metadataStart = version.IndexOf('+');
        return metadataStart < 0 ? version : version[..metadataStart];
    }
}
