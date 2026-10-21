// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Vela.UI.Cli.Install;
using Vela.UI.Cli.Project;
using Vela.UI.Cli.Registry;

namespace Vela.UI.Cli.Commands;

/// <summary>
/// <c>vela add</c>: copies components, and the components they use, into the project.
/// </summary>
/// <remarks>
/// A component that another one needs is skipped if its folder already exists, so an
/// installed and edited Button isn't touched when Pagination is added. Components named on
/// the command line are always written, subject to the overwrite rules in
/// <see cref="FileInstaller"/>.
/// </remarks>
internal static class AddCommand
{
    public const string PrimitivesPackage = "Vela.UI.Primitives";

    public static int Run(string directory, IReadOnlyList<string> names, bool overwrite, TextWriter output, TextWriter error)
    {
        var registry = BundledRegistry.Instance;
        var config = VelaConfig.Load(directory);

        var requested = names
            .Select(name => registry.Find(name) ?? throw new CliException(
                $"There's no component called '{name}'. Run 'vela --help' to see the list."))
            .Distinct()
            .ToList();

        var install = new List<RegistryComponent>();
        var kept = new List<RegistryComponent>();
        foreach (var component in requested)
        {
            Visit(component);
        }

        void Visit(RegistryComponent component)
        {
            if (install.Contains(component) || kept.Contains(component))
            {
                return;
            }

            foreach (var dependency in component.Dependencies)
            {
                Visit(registry.Find(dependency)!);
            }

            var folder = Path.Combine(directory, config.Components, component.Folder);
            if (!requested.Contains(component) && Directory.Exists(folder))
            {
                kept.Add(component);
                return;
            }

            install.Add(component);
        }

        var files = install
            .SelectMany(component => component.Files.Select(file => new PlannedFile(
                $"{config.Components}/{component.Folder}/{file.Path}",
                FileInstaller.RewriteNamespace(file.Content, config.Namespace))))
            .ToList();

        output.WriteLine($"Adding {string.Join(", ", install.Select(component => component.Name))}:");
        var exitCode = FileInstaller.Install(directory, files, overwrite, output, error);
        if (exitCode != VelaCli.Success)
        {
            return exitCode;
        }

        foreach (var component in kept)
        {
            output.WriteLine($"Kept your existing {component.Name}, which the new components use.");
        }

        if (install.Any(component => component.UsesPrimitives))
        {
            var project = VelaProject.Find(directory, registry.TargetFramework);
            if (project.AddPackageReference(PrimitivesPackage, VelaCli.GetVersion()) is { } note)
            {
                output.WriteLine(note);
            }
        }

        return VelaCli.Success;
    }
}
