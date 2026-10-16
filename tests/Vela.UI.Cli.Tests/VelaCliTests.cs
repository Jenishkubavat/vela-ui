using System.Text.RegularExpressions;

namespace Vela.UI.Cli.Tests;

public class VelaCliTests
{
    [Theory]
    [InlineData]
    [InlineData("--help")]
    [InlineData("-h")]
    public void Help_is_printed_to_output_and_succeeds(params string[] args)
    {
        var (exitCode, output, error) = Run(args);

        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Contains("Usage: vela", output);
        Assert.Empty(error);
    }

    [Fact]
    public void Version_is_a_semantic_version_without_build_metadata()
    {
        var (exitCode, output, _) = Run("--version");

        Assert.Equal(VelaCli.Success, exitCode);
        Assert.Matches(new Regex(@"^\d+\.\d+\.\d+(-[0-9A-Za-z.-]+)?$"), output.Trim());
    }

    [Fact]
    public void Unknown_argument_is_reported_on_error_with_usage_error_code()
    {
        var (exitCode, output, error) = Run("frobnicate");

        Assert.Equal(VelaCli.UsageError, exitCode);
        Assert.Empty(output);
        Assert.Contains("frobnicate", error);
    }

    private static (int ExitCode, string Output, string Error) Run(params string[] args)
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = VelaCli.Run(args, output, error);
        return (exitCode, output.ToString(), error.ToString());
    }
}
