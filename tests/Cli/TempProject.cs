// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Cli.Tests;

/// <summary>
/// A throwaway project folder for running the CLI against. Deleted when disposed.
/// </summary>
internal sealed class TempProject : IDisposable
{
    public const string BlazorProject = """
        <Project Sdk="Microsoft.NET.Sdk.Web">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """;

    public TempProject(string projectFile = "DemoApp.csproj", string? content = BlazorProject)
    {
        Directory = Path.Combine(Path.GetTempPath(), "vela-tests", Guid.NewGuid().ToString("N"));
        System.IO.Directory.CreateDirectory(Directory);
        if (content is not null)
        {
            Write(projectFile, content);
        }
    }

    public string Directory { get; }

    public string Read(string path) => File.ReadAllText(FullPath(path));

    public bool Exists(string path) => File.Exists(FullPath(path));

    public void Write(string path, string content)
    {
        var fullPath = FullPath(path);
        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, content);
    }

    public (int ExitCode, string Output, string Error) Run(params string[] args)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = VelaCli.Run([.. args, "--cwd", Directory], output, error);
        return (exitCode, output.ToString(), error.ToString());
    }

    public void Dispose()
    {
        try
        {
            System.IO.Directory.Delete(Directory, recursive: true);
        }
        catch (IOException)
        {
            // A leftover temp folder isn't worth failing a test over.
        }
    }

    private string FullPath(string path) => Path.Combine(Directory, path.Replace('/', Path.DirectorySeparatorChar));
}
