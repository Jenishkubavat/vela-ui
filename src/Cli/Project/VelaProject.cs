// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Vela.UI.Cli.Project;

/// <summary>
/// The Blazor project that components are installed into.
/// </summary>
internal sealed partial class VelaProject
{
    private static readonly string[] BlazorSdks =
        ["Microsoft.NET.Sdk.Web", "Microsoft.NET.Sdk.Razor", "Microsoft.NET.Sdk.BlazorWebAssembly"];

    private VelaProject(string directory, string projectFile, XDocument document)
    {
        Directory = directory;
        ProjectFile = projectFile;
        RootNamespace = ReadProperty(document, "RootNamespace")
            ?? ToNamespace(Path.GetFileNameWithoutExtension(projectFile));
    }

    /// <summary>
    /// The folder that holds the project file.
    /// </summary>
    public string Directory { get; }

    /// <summary>
    /// The project file's name, such as <c>MyApp.csproj</c>.
    /// </summary>
    public string ProjectFile { get; }

    /// <summary>
    /// The project's root namespace, from <c>RootNamespace</c> or its file name.
    /// </summary>
    public string RootNamespace { get; }

    /// <summary>
    /// Finds the one Blazor project in <paramref name="directory"/>, or explains why it can't.
    /// </summary>
    public static VelaProject Find(string directory, string targetFramework)
    {
        if (!System.IO.Directory.Exists(directory))
        {
            throw new CliException($"The folder '{directory}' doesn't exist.");
        }

        var projects = System.IO.Directory.GetFiles(directory, "*.csproj");
        if (projects.Length == 0)
        {
            throw new CliException(
                $"No .csproj file in '{directory}'. Run vela in your Blazor project's folder, or pass it with --cwd.");
        }

        if (projects.Length > 1)
        {
            var names = string.Join(", ", projects.Select(Path.GetFileName));
            throw new CliException($"More than one project in '{directory}' ({names}). Run vela in a folder with one project.");
        }

        var projectFile = Path.GetFileName(projects[0]);
        var document = XDocument.Load(projects[0]);

        var sdk = document.Root?.Attribute("Sdk")?.Value ?? "";
        if (!BlazorSdks.Any(blazorSdk => sdk.StartsWith(blazorSdk, StringComparison.OrdinalIgnoreCase)))
        {
            throw new CliException(
                $"{projectFile} isn't a Blazor project (its SDK is '{sdk}'). Vela UI needs a Blazor Web App, " +
                "a Blazor WebAssembly app, or a Razor class library.");
        }

        CheckTargetFramework(document, projectFile, targetFramework);
        return new VelaProject(directory, projectFile, document);
    }

    /// <summary>
    /// Adds a <c>PackageReference</c> unless the project already references the package, or
    /// the project of the same name (when working against a local copy). Returns a note for
    /// the user, or <see langword="null"/> when nothing changed.
    /// </summary>
    /// <remarks>
    /// The project file is edited as text so its formatting and comments are kept. With
    /// central package management the version belongs in <c>Directory.Packages.props</c>,
    /// which isn't edited; the note says what to add there.
    /// </remarks>
    public string? AddPackageReference(string package, string version)
    {
        var path = Path.Combine(Directory, ProjectFile);
        var text = File.ReadAllText(path);
        var document = XDocument.Parse(text);
        var hasReference = document.Descendants().Any(element =>
            (element.Name.LocalName == "PackageReference"
                && string.Equals((string?)element.Attribute("Include"), package, StringComparison.OrdinalIgnoreCase))
            || (element.Name.LocalName == "ProjectReference"
                && string.Equals(Path.GetFileNameWithoutExtension((string?)element.Attribute("Include")), package, StringComparison.OrdinalIgnoreCase)));
        if (hasReference)
        {
            return null;
        }

        var centralProps = FindCentralPackageProps();
        var reference = centralProps is null
            ? $"<PackageReference Include=\"{package}\" Version=\"{version}\" />"
            : $"<PackageReference Include=\"{package}\" />";

        var newline = text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var block = new StringBuilder()
            .Append("  <ItemGroup>").Append(newline)
            .Append("    ").Append(reference).Append(newline)
            .Append("  </ItemGroup>").Append(newline)
            .Append(newline)
            .ToString();
        var end = text.LastIndexOf("</Project>", StringComparison.Ordinal);
        File.WriteAllText(path, text.Insert(end, block));

        return centralProps is null
            ? $"Added the {package} package to {ProjectFile}."
            : $"Added the {package} package to {ProjectFile}. Your solution manages package versions centrally, " +
              $"so add <PackageVersion Include=\"{package}\" Version=\"{version}\" /> to {centralProps}.";
    }

    private string? FindCentralPackageProps()
    {
        for (var folder = new DirectoryInfo(Directory); folder is not null; folder = folder.Parent)
        {
            var props = Path.Combine(folder.FullName, "Directory.Packages.props");
            if (File.Exists(props))
            {
                var enabled = ReadProperty(XDocument.Load(props), "ManagePackageVersionsCentrally");
                return string.Equals(enabled, "true", StringComparison.OrdinalIgnoreCase) ? props : null;
            }
        }

        return null;
    }

    // Only checks a framework set in the project file itself; one inherited from
    // Directory.Build.props is left to the build.
    private static void CheckTargetFramework(XDocument document, string projectFile, string required)
    {
        var frameworks = ReadProperty(document, "TargetFramework") ?? ReadProperty(document, "TargetFrameworks");
        if (frameworks is null)
        {
            return;
        }

        var minimum = ParseNetVersion(required)!;
        var supported = frameworks.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(framework => ParseNetVersion(framework) is { } version && version >= minimum);
        if (!supported)
        {
            throw new CliException(
                $"{projectFile} targets {frameworks}. Vela UI components need {required} or later; " +
                $"update <TargetFramework> in {projectFile}.");
        }
    }

    private static Version? ParseNetVersion(string framework)
    {
        var match = NetVersion().Match(framework);
        return match.Success ? Version.Parse(match.Groups[1].Value) : null;
    }

    private static string? ReadProperty(XDocument document, string name) =>
        document.Descendants()
            .Where(element => element.Name.LocalName == name)
            .Select(element => element.Value.Trim())
            .LastOrDefault(value => value.Length > 0);

    // The SDK's default root namespace is the project name with characters that aren't valid in
    // an identifier replaced by underscores.
    private static string ToNamespace(string projectName) =>
        string.Join('.', projectName.Split('.').Select(part =>
        {
            var identifier = InvalidIdentifierCharacters().Replace(part, "_");
            return identifier.Length == 0 || char.IsDigit(identifier[0]) ? "_" + identifier : identifier;
        }));

    [GeneratedRegex(@"^net(\d+\.\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex NetVersion();

    [GeneratedRegex(@"[^\p{L}\p{Nd}_]")]
    private static partial Regex InvalidIdentifierCharacters();
}
