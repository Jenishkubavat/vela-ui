// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Cli.Tests;

public class InitCommandTests
{
    [Fact]
    public void Init_WritesTheSharedFilesAndConfig()
    {
        // Arrange
        using var project = new TempProject();

        // Act
        var (exitCode, output, error) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Empty(error);
        Assert.True(project.Exists("Styles/vela.css"));
        Assert.True(project.Exists("wwwroot/fonts/HostGrotesk-latin.woff2"));
        Assert.True(project.Exists("wwwroot/fonts/SplineSansMono-OFL.txt"));
        Assert.True(project.Exists("wwwroot/vela-light.js"));
        Assert.Contains("namespace DemoApp.Components.Ui;", project.Read("Components/Ui/ClassNames.cs"));
        Assert.Contains("\"namespace\": \"DemoApp.Components.Ui\"", project.Read("vela.json"));
        Assert.Contains("\"project\": \"DemoApp.csproj\"", project.Read("vela.json"));
        Assert.Contains("@using DemoApp.Components.Ui", output);
    }

    [Fact]
    public void Init_TailwindInputReadsTheComponentSource()
    {
        // Arrange
        using var project = new TempProject();

        // Act
        project.Run("init");

        // Assert
        var input = project.Read("Styles/app.css");
        Assert.Contains("@import \"tailwindcss\";", input);
        Assert.Contains("@import \"./vela.css\";", input);
        Assert.Contains("@source \"../Components/Ui\";", input);
    }

    [Fact]
    public void Init_UsesTheRootNamespaceProperty()
    {
        // Arrange
        using var project = new TempProject(content: TempProject.BlazorProject.Replace(
            "</TargetFramework>", "</TargetFramework>\n    <RootNamespace>Contoso.Web</RootNamespace>"));

        // Act
        project.Run("init");

        // Assert
        Assert.Contains("namespace Contoso.Web.Components.Ui;", project.Read("Components/Ui/ClassNames.cs"));
    }

    [Fact]
    public void Init_MakesAProjectNameIntoAValidNamespace()
    {
        // Arrange
        using var project = new TempProject("my-app.2.csproj");

        // Act
        project.Run("init");

        // Assert
        Assert.Contains("namespace my_app._2.Components.Ui;", project.Read("Components/Ui/ClassNames.cs"));
    }

    [Fact]
    public void Init_TwiceChangesNothing()
    {
        // Arrange
        using var project = new TempProject();
        project.Run("init");

        // Act
        var (exitCode, output, _) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Contains("already up to date", output);
    }

    [Fact]
    public void Init_StopsBeforeReplacingAnEditedFile()
    {
        // Arrange
        using var project = new TempProject();
        project.Run("init");
        project.Write("Styles/vela.css", "/* my theme */");
        File.Delete(Path.Combine(project.Directory, "wwwroot", "vela-light.js"));

        // Act
        var (exitCode, _, error) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Conflict, exitCode);
        Assert.Contains("Styles/vela.css", error);
        Assert.Equal("/* my theme */", project.Read("Styles/vela.css"));
        Assert.False(project.Exists("wwwroot/vela-light.js"));
    }

    [Fact]
    public void Init_OverwriteKeepsABackup()
    {
        // Arrange
        using var project = new TempProject();
        project.Run("init");
        project.Write("Styles/vela.css", "/* my theme */");

        // Act
        var (exitCode, _, _) = project.Run("init", "--overwrite");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Contains("@font-face", project.Read("Styles/vela.css"));
        var backup = Directory.GetFiles(Path.Combine(project.Directory, ".vela", "backups"), "vela.css", SearchOption.AllDirectories);
        Assert.Equal("/* my theme */", File.ReadAllText(Assert.Single(backup)));
    }

    [Fact]
    public void Init_NeedsAProject()
    {
        // Arrange
        using var project = new TempProject(content: null);

        // Act
        var (exitCode, _, error) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Failure, exitCode);
        Assert.Contains("No .csproj file", error);
    }

    [Fact]
    public void Init_NeedsExactlyOneProject()
    {
        // Arrange
        using var project = new TempProject();
        project.Write("Other.csproj", TempProject.BlazorProject);

        // Act
        var (exitCode, _, error) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Failure, exitCode);
        Assert.Contains("More than one project", error);
    }

    [Fact]
    public void Init_RejectsAProjectThatIsNotBlazor()
    {
        // Arrange
        using var project = new TempProject(content: TempProject.BlazorProject.Replace("Microsoft.NET.Sdk.Web", "Microsoft.NET.Sdk"));

        // Act
        var (exitCode, _, error) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Failure, exitCode);
        Assert.Contains("isn't a Blazor project", error);
        Assert.False(project.Exists("vela.json"));
    }

    [Fact]
    public void Init_RejectsAnOlderFramework()
    {
        // Arrange
        using var project = new TempProject(content: TempProject.BlazorProject.Replace("net10.0", "net8.0"));

        // Act
        var (exitCode, _, error) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Failure, exitCode);
        Assert.Contains("net10.0 or later", error);
    }

    [Fact]
    public void Init_AcceptsANewerFrameworkAmongSeveral()
    {
        // Arrange
        using var project = new TempProject(content: TempProject.BlazorProject
            .Replace("<TargetFramework>net10.0</TargetFramework>", "<TargetFrameworks>net8.0;net11.0</TargetFrameworks>"));

        // Act
        var (exitCode, _, _) = project.Run("init");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
    }
}
