// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace Vela.UI.Cli.Tests;

public class VelaCliTests
{
    [Theory]
    [InlineData]
    [InlineData("--help")]
    [InlineData("-h")]
    public void VelaCli_Help(params string[] args)
    {
        // Arrange && Act
        var (exitCode, output, error) = Run(args);

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Contains("Usage: vela", output);
        Assert.Empty(error);
    }

    [Fact]
    public void VelaCli_Version()
    {
        // Arrange && Act
        var (exitCode, output, _) = Run("--version");

        // Assert
        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Matches(new Regex(@"^\d+\.\d+\.\d+(-[0-9A-Za-z.-]+)?$"), output.Trim());
    }

    [Fact]
    public void VelaCli_UnknownArgument()
    {
        // Arrange && Act
        var (exitCode, output, error) = Run("frobnicate");

        // Assert
        Assert.Equal(VelaCli.UsageError, exitCode);
        Assert.Empty(output);
        Assert.Contains("frobnicate", error);
    }

    [Fact]
    public void VelaCli_HelpListsTheComponents()
    {
        // Arrange && Act
        var (_, output, _) = Run("--help");

        // Assert
        Assert.Contains("init", output);
        Assert.Contains("add <component>", output);
        Assert.Contains("input-group", output);
        Assert.All(output.Split('\n'), line => Assert.True(line.TrimEnd().Length <= 100, line));
    }

    [Theory]
    [InlineData("add")]
    [InlineData("init", "button")]
    [InlineData("add", "button", "--force")]
    [InlineData("init", "--cwd")]
    public void VelaCli_BadArgumentsAreAUsageError(params string[] args)
    {
        // Arrange && Act
        var (exitCode, output, error) = Run(args);

        // Assert
        Assert.Equal(VelaCli.UsageError, exitCode);
        Assert.Empty(output);
        Assert.Contains("vela --help", error);
    }

    private static (int ExitCode, string Output, string Error) Run(params string[] args)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = VelaCli.Run(args, output, error);
        return (exitCode, output.ToString(), error.ToString());
    }
}
