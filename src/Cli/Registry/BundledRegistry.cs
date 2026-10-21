// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;

namespace Vela.UI.Cli.Registry;

/// <summary>
/// The component registry embedded in the CLI: the source of every component in
/// <c>src/Core</c>, the shared files <c>vela init</c> installs, and <c>registry.json</c>.
/// </summary>
internal sealed class BundledRegistry
{
    private const string Prefix = "registry/";

    private readonly Dictionary<string, byte[]> _resources;
    private readonly Dictionary<string, RegistryComponent> _components;

    private BundledRegistry(Dictionary<string, byte[]> resources)
    {
        _resources = resources;

        using var manifest = JsonDocument.Parse(_resources["registry.json"]);
        TargetFramework = manifest.RootElement.GetProperty("targetFramework").GetString()!;

        _components = new(StringComparer.OrdinalIgnoreCase);
        foreach (var entry in manifest.RootElement.GetProperty("components").EnumerateObject())
        {
            var folder = ToFolderName(entry.Name);
            var dependencies = entry.Value.TryGetProperty("dependencies", out var list)
                ? list.EnumerateArray().Select(item => item.GetString()!).ToArray()
                : [];
            var files = FilesUnder($"Components/{folder}/");
            _components[entry.Name] = new RegistryComponent(entry.Name, folder, dependencies, files);
        }
    }

    /// <summary>
    /// The registry built into this copy of the CLI.
    /// </summary>
    public static BundledRegistry Instance { get; } = Load(typeof(BundledRegistry).Assembly);

    /// <summary>
    /// The oldest target framework the components are built and tested against, such as
    /// <c>net10.0</c>.
    /// </summary>
    public string TargetFramework { get; }

    /// <summary>
    /// Every component, by name.
    /// </summary>
    public IEnumerable<RegistryComponent> Components => _components.Values.OrderBy(c => c.Name, StringComparer.Ordinal);

    /// <summary>
    /// The component with this name, ignoring case, or <see langword="null"/>.
    /// </summary>
    public RegistryComponent? Find(string name) => _components.GetValueOrDefault(name);

    /// <summary>
    /// A shared file by its path under <c>src/Core</c>, such as <c>Styles/vela.css</c>.
    /// </summary>
    public byte[] GetFile(string path) => _resources[path];

    /// <summary>
    /// The files under a folder of <c>src/Core</c>, with paths relative to it.
    /// </summary>
    public IReadOnlyList<RegistryFile> FilesUnder(string folder) =>
        _resources
            .Where(resource => resource.Key.StartsWith(folder, StringComparison.Ordinal))
            .OrderBy(resource => resource.Key, StringComparer.Ordinal)
            .Select(resource => new RegistryFile(resource.Key[folder.Length..], resource.Value))
            .ToArray();

    /// <summary>
    /// Turns a command-line name into its folder name: <c>input-group</c> becomes <c>InputGroup</c>.
    /// </summary>
    public static string ToFolderName(string name) =>
        string.Concat(name.Split('-').Select(part => char.ToUpperInvariant(part[0]) + part[1..]));

    private static BundledRegistry Load(Assembly assembly)
    {
        var resources = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        foreach (var name in assembly.GetManifestResourceNames())
        {
            // MSBuild writes RecursiveDir with the build machine's separator.
            var path = name.Replace('\\', '/');
            if (!path.StartsWith(Prefix, StringComparison.Ordinal))
            {
                continue;
            }

            using var stream = assembly.GetManifestResourceStream(name)!;
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            resources[path[Prefix.Length..]] = buffer.ToArray();
        }

        return new BundledRegistry(resources);
    }
}
