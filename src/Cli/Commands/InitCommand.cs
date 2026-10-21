// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using Vela.UI.Cli.Install;
using Vela.UI.Cli.Project;
using Vela.UI.Cli.Registry;

namespace Vela.UI.Cli.Commands;

/// <summary>
/// <c>vela init</c>: installs the files every component needs (<c>ClassNames</c>, the
/// theme, fonts, and the motion layer), a Tailwind input file, and <c>vela.json</c>.
/// </summary>
internal static class InitCommand
{
    public static int Run(string directory, bool overwrite, TextWriter output, TextWriter error)
    {
        var registry = BundledRegistry.Instance;
        var project = VelaProject.Find(directory, registry.TargetFramework);
        var config = VelaConfig.CreateDefault(project, VelaCli.GetVersion());

        List<PlannedFile> files =
        [
            new($"{config.Components}/ClassNames.cs",
                FileInstaller.RewriteNamespace(registry.GetFile("Utilities/ClassNames.cs"), config.Namespace)),
            new($"{config.Styles}/vela.css", registry.GetFile("Styles/vela.css")),
            new($"{config.Styles}/app.css", TailwindInput(config)),
            .. registry.FilesUnder("Styles/fonts/").Select(font => new PlannedFile($"{config.Assets}/fonts/{font.Path}", font.Content)),
            new($"{config.Assets}/vela-light.js", registry.GetFile("Scripts/vela-light.js")),
            new(VelaConfig.FileName, config.ToUtf8Bytes()),
        ];

        output.WriteLine($"Setting up Vela UI in {project.ProjectFile}:");
        var exitCode = FileInstaller.Install(directory, files, overwrite, output, error);
        if (exitCode != VelaCli.Success)
        {
            return exitCode;
        }

        output.WriteLine();
        output.WriteLine(NextSteps(config));
        return VelaCli.Success;
    }

    private static string CompiledCss(VelaConfig config) => $"{config.Assets}/tailwind.css";

    private static byte[] TailwindInput(VelaConfig config)
    {
        var components = Path.GetRelativePath(config.Styles, config.Components).Replace('\\', '/');
        return Encoding.UTF8.GetBytes($$"""
            /*
              Tailwind CSS input for this app. Build it with Tailwind CSS v4:

                npm install tailwindcss @tailwindcss/cli
                npx @tailwindcss/cli -i {{config.Styles}}/app.css -o {{CompiledCss(config)}} --watch

              Tailwind doesn't move font files, so {{config.Assets}}/fonts/ must stay next to the
              compiled CSS.
            */
            @import "tailwindcss";
            @import "./vela.css";

            /* Tailwind has to read class names from the component source, not the rendered HTML. */
            @source "{{components}}";

            """);
    }

    private static string NextSteps(VelaConfig config) => $$"""
        Next steps:
          1. Install Tailwind CSS v4 in this folder, then build the CSS and keep it running while you work:
               npm install tailwindcss @tailwindcss/cli
               npx @tailwindcss/cli -i {{config.Styles}}/app.css -o {{CompiledCss(config)}} --watch
          2. In App.razor, link it in <head>. Optionally, load the motion layer before </body>:
               <link rel="stylesheet" href="tailwind.css" />
               <script src="vela-light.js"></script>
          3. Add this line to Components/_Imports.razor:
               @using {{config.Namespace}}

        Then add components, for example: vela add button
        """;
}
