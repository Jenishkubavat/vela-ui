// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace Vela.UI.Cli.Tests;

public class AddCommandTests
{
    [Fact]
    public void Add_CopiesTheComponentIntoTheAppsNamespace()
    {
        // Arrange
        using var project = Initialized();

        // Act
        var (exitCode, output, error) = project.Run("add", "button");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Empty(error);
        Assert.Contains("Components/Ui/Button/Button.razor", output);
        Assert.StartsWith("@namespace DemoApp.Components.Ui", project.Read("Components/Ui/Button/Button.razor"));
        Assert.Contains("namespace DemoApp.Components.Ui;", project.Read("Components/Ui/Button/ButtonVariant.cs"));
        Assert.DoesNotContain("Vela.UI.Components", project.Read("Components/Ui/Button/Button.razor.cs"));
    }

    [Fact]
    public void Add_InstallsTheComponentsItUses()
    {
        // Arrange
        using var project = Initialized();

        // Act
        var (exitCode, _, _) = project.Run("add", "pagination");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.True(project.Exists("Components/Ui/Pagination/PaginationLink.razor"));
        Assert.True(project.Exists("Components/Ui/Button/Button.razor"));
    }

    [Fact]
    public void Add_KeepsAnEditedComponentThatAnotherOneUses()
    {
        // Arrange
        using var project = Initialized();
        project.Run("add", "button");
        project.Write("Components/Ui/Button/Button.razor", "@* my button *@");

        // Act
        var (exitCode, output, _) = project.Run("add", "pagination");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Contains("Kept your existing button", output);
        Assert.Equal("@* my button *@", project.Read("Components/Ui/Button/Button.razor"));
        Assert.True(project.Exists("Components/Ui/Pagination/Pagination.razor"));
    }

    [Fact]
    public void Add_StopsBeforeReplacingAnEditedComponent()
    {
        // Arrange
        using var project = Initialized();
        project.Run("add", "button");
        project.Write("Components/Ui/Button/Button.razor", "@* my button *@");

        // Act
        var (exitCode, _, error) = project.Run("add", "button");

        // Assert
        Assert.Equal(VelaCli.Conflict, exitCode);
        Assert.Contains("Components/Ui/Button/Button.razor", error);
        Assert.Equal("@* my button *@", project.Read("Components/Ui/Button/Button.razor"));
    }

    [Fact]
    public void Add_OverwriteReplacesAnEditedComponent()
    {
        // Arrange
        using var project = Initialized();
        project.Run("add", "button");
        project.Write("Components/Ui/Button/Button.razor", "@* my button *@");

        // Act
        var (exitCode, output, _) = project.Run("add", "button", "--overwrite");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Contains(".vela/backups", output);
        Assert.StartsWith("@namespace", project.Read("Components/Ui/Button/Button.razor"));
    }

    [Fact]
    public void Add_PrimitiveBasedComponentAddsThePackageOnce()
    {
        // Arrange
        using var project = Initialized();

        // Act
        var (_, output, _) = project.Run("add", "label");
        project.Run("add", "checkbox");

        // Assert
        Assert.Contains("Added the Vela.UI.Primitives package", output);
        var csproj = project.Read("DemoApp.csproj");
        Assert.Single(Regex.Matches(csproj, "Include=\"Vela.UI.Primitives\""));
        Assert.Contains($"Version=\"{VelaCli.GetVersion()}\"", csproj);
    }

    [Fact]
    public void Add_StyledOnlyComponentLeavesTheProjectFileAlone()
    {
        // Arrange
        using var project = Initialized();
        var before = project.Read("DemoApp.csproj");

        // Act
        project.Run("add", "card");

        // Assert
        Assert.Equal(before, project.Read("DemoApp.csproj"));
    }

    [Fact]
    public void Add_WithCentralPackageManagementLeavesTheVersionOut()
    {
        // Arrange
        using var project = Initialized();
        project.Write("Directory.Packages.props", """
            <Project>
              <PropertyGroup>
                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
              </PropertyGroup>
            </Project>
            """);

        // Act
        var (_, output, _) = project.Run("add", "switch");

        // Assert
        Assert.Contains("<PackageReference Include=\"Vela.UI.Primitives\" />", project.Read("DemoApp.csproj"));
        Assert.Contains("<PackageVersion Include=\"Vela.UI.Primitives\"", output);
    }

    [Fact]
    public void Add_LocalProjectReferenceCountsAsThePackage()
    {
        // Arrange
        using var project = Initialized(TempProject.BlazorProject.Replace(
            "</Project>", "  <ItemGroup>\n    <ProjectReference Include=\"..\\Vela.UI.Primitives\\Vela.UI.Primitives.csproj\" />\n  </ItemGroup>\n</Project>"));

        // Act
        project.Run("add", "toggle");

        // Assert
        Assert.DoesNotContain("PackageReference", project.Read("DemoApp.csproj"));
    }

    [Fact]
    public void Add_UnknownComponentWritesNothing()
    {
        // Arrange
        using var project = Initialized();

        // Act
        var (exitCode, _, error) = project.Run("add", "button", "frobnicator");

        // Assert
        Assert.Equal(VelaCli.Failure, exitCode);
        Assert.Contains("'frobnicator'", error);
        Assert.False(project.Exists("Components/Ui/Button/Button.razor"));
    }

    [Fact]
    public void Add_NeedsInit()
    {
        // Arrange
        using var project = new TempProject();

        // Act
        var (exitCode, _, error) = project.Run("add", "button");

        // Assert
        Assert.Equal(VelaCli.Failure, exitCode);
        Assert.Contains("Run 'vela init' first", error);
    }

    [Fact]
    public void Add_FollowsThePathsInVelaJson()
    {
        // Arrange
        using var project = Initialized();
        project.Write("vela.json", project.Read("vela.json")
            .Replace("\"Components/Ui\"", "\"Shared/Vela\"")
            .Replace("DemoApp.Components.Ui", "DemoApp.Shared.Vela"));

        // Act
        project.Run("add", "Badge");

        // Assert
        Assert.StartsWith("@namespace DemoApp.Shared.Vela", project.Read("Shared/Vela/Badge/Badge.razor"));
    }

    private static TempProject Initialized(string content = TempProject.BlazorProject)
    {
        var project = new TempProject(content: content);
        Assert.Equal(VelaCli.Success, project.Run("init").ExitCode);
        return project;
    }
}
