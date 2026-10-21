// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Cli.Registry;

/// <summary>
/// One component in the registry: the files in its folder and the other components it
/// uses.
/// </summary>
/// <param name="Name">The name used on the command line, such as <c>input-group</c>.</param>
/// <param name="Folder">The folder it's installed into, such as <c>InputGroup</c>.</param>
/// <param name="Dependencies">Names of the components it uses, from <c>registry.json</c>.</param>
/// <param name="Files">Its source files, relative to <paramref name="Folder"/>.</param>
internal sealed record RegistryComponent(
    string Name,
    string Folder,
    IReadOnlyList<string> Dependencies,
    IReadOnlyList<RegistryFile> Files)
{
    /// <summary>
    /// Whether the component is built on a headless primitive, so the app needs the
    /// Vela.UI.Primitives package.
    /// </summary>
    public bool UsesPrimitives => Files.Any(file =>
        System.Text.Encoding.UTF8.GetString(file.Content).Contains("using Vela.UI.Primitives", StringComparison.Ordinal));
}
