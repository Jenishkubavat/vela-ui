// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Vela.UI.Cli.Project;

/// <summary>
/// The contents of <c>vela.json</c>, written by <c>vela init</c> and read by <c>vela add</c>.
/// Paths are relative to the project folder and use forward slashes.
/// </summary>
/// <param name="Project">The project file components are installed into.</param>
/// <param name="Namespace">The namespace installed components are given.</param>
/// <param name="Components">Where components go.</param>
/// <param name="Styles">Where <c>vela.css</c> and the Tailwind input file go.</param>
/// <param name="Assets">The web root, where fonts and <c>vela-light.js</c> go.</param>
/// <param name="Registry">Where components come from: the registry bundled in the CLI.</param>
/// <param name="Version">The version of the registry the project was set up with.</param>
internal sealed partial record VelaConfig(
    string Project,
    string Namespace,
    string Components,
    string Styles,
    string Assets,
    string Registry,
    string Version)
{
    /// <summary>
    /// The configuration file's name.
    /// </summary>
    public const string FileName = "vela.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    /// <summary>
    /// The configuration <c>vela init</c> writes for a project.
    /// </summary>
    public static VelaConfig CreateDefault(VelaProject project, string version) => new(
        Project: project.ProjectFile,
        Namespace: $"{project.RootNamespace}.Components.Ui",
        Components: "Components/Ui",
        Styles: "Styles",
        Assets: "wwwroot",
        Registry: "bundled",
        Version: version);

    /// <summary>
    /// Reads <c>vela.json</c> from <paramref name="directory"/>, or explains why it can't.
    /// </summary>
    public static VelaConfig Load(string directory)
    {
        var path = Path.Combine(directory, FileName);
        if (!File.Exists(path))
        {
            throw new CliException($"No {FileName} in '{directory}'. Run 'vela init' first.");
        }

        VelaConfig? config;
        try
        {
            config = JsonSerializer.Deserialize<VelaConfig>(File.ReadAllText(path), JsonOptions);
        }
        catch (JsonException exception)
        {
            throw new CliException($"{FileName} isn't valid JSON: {exception.Message}");
        }

        string?[] required = [config?.Project, config?.Namespace, config?.Components, config?.Styles, config?.Assets];
        if (config is null || required.Any(string.IsNullOrWhiteSpace))
        {
            throw new CliException(
                $"{FileName} is missing a setting. It needs project, namespace, components, styles, and assets; " +
                "delete it and run 'vela init' to recreate it.");
        }

        if (!DottedIdentifier().IsMatch(config.Namespace))
        {
            throw new CliException(
                $"The namespace in {FileName} ('{config.Namespace}') isn't a valid C# namespace. " +
                "Use names separated by dots, such as MyApp.Components.Ui.");
        }

        CheckInsideProject(directory, "components", config.Components);
        CheckInsideProject(directory, "styles", config.Styles);
        CheckInsideProject(directory, "assets", config.Assets);
        return config;
    }

    /// <summary>
    /// The file's contents, ending with a newline.
    /// </summary>
    public byte[] ToUtf8Bytes() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this, JsonOptions) + "\n");

    // The CLI writes into these folders, so a path like "../x" or "C:/x" would let it write
    // anywhere on the machine.
    private static void CheckInsideProject(string directory, string setting, string path)
    {
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory)) + Path.DirectorySeparatorChar;
        var target = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.Combine(root, path))) + Path.DirectorySeparatorChar;
        if (Path.IsPathRooted(path) || !target.StartsWith(root, comparison))
        {
            throw new CliException(
                $"The {setting} folder in {FileName} ('{path}') is outside the project. " +
                "Use a path relative to the project folder, such as Components/Ui.");
        }
    }

    [GeneratedRegex(@"^[\p{L}_][\p{L}\p{Nd}_]*(\.[\p{L}_][\p{L}\p{Nd}_]*)*$")]
    private static partial Regex DottedIdentifier();
}
