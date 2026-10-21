// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;
using Vela.UI.Cli.Registry;

namespace Vela.UI.Cli.Tests;

public class BundledRegistryTests
{
    private static readonly BundledRegistry Registry = BundledRegistry.Instance;

    [Fact]
    public void BundledRegistry_ListsEveryComponentFolder()
    {
        // Arrange
        var folders = Registry.FilesUnder("Components/")
            .Select(file => file.Path.Split('/')[0])
            .Distinct()
            .Order(StringComparer.Ordinal);

        // Act
        var listed = Registry.Components.Select(component => component.Folder).Order(StringComparer.Ordinal);

        // Assert
        Assert.Equal(folders, listed);
        Assert.All(Registry.Components, component => Assert.NotEmpty(component.Files));
    }

    [Fact]
    public void BundledRegistry_DependenciesAreComponents()
    {
        foreach (var component in Registry.Components)
        {
            // Act && Assert
            Assert.All(component.Dependencies, dependency => Assert.NotNull(Registry.Find(dependency)));
        }
    }

    // Keeps registry.json honest: a component that uses another one's tag (<Button>), static
    // members (Button.GetClasses), or types (ButtonSize Size) must declare it, and nothing else.
    [Fact]
    public void BundledRegistry_DeclaresExactlyTheComponentsEachOneUses()
    {
        var typesByComponent = Registry.Components.ToDictionary(
            component => component.Name,
            component => component.Files.Select(file => file.Path.Split('.')[0]).Distinct().ToArray());

        foreach (var component in Registry.Components)
        {
            // Arrange
            var code = CodeWithoutCommentsOrStrings(component);

            // Act
            var used = typesByComponent
                .Where(other => other.Key != component.Name && other.Value.Any(type => Uses(code, type)))
                .Select(other => other.Key)
                .Order(StringComparer.Ordinal);

            // Assert
            Assert.True(
                used.SequenceEqual(component.Dependencies.Order(StringComparer.Ordinal)),
                $"{component.Name} uses [{string.Join(", ", used)}] but registry.json declares [{string.Join(", ", component.Dependencies)}].");
        }
    }

    [Fact]
    public void BundledRegistry_IncludesTheSharedFiles()
    {
        // Act && Assert
        Assert.Contains("public static class ClassNames", Encoding.UTF8.GetString(Registry.GetFile("Utilities/ClassNames.cs")));
        Assert.Contains("@font-face", Encoding.UTF8.GetString(Registry.GetFile("Styles/vela.css")));
        Assert.NotEmpty(Registry.GetFile("Scripts/vela-light.js"));
        Assert.Equal(4, Registry.FilesUnder("Styles/fonts/").Count);
    }

    [Theory]
    [InlineData("button", "Button")]
    [InlineData("input-group", "InputGroup")]
    [InlineData("aspect-ratio", "AspectRatio")]
    public void BundledRegistry_ToFolderName(string name, string expected)
    {
        // Act && Assert
        Assert.Equal(expected, BundledRegistry.ToFolderName(name));
    }

    [Fact]
    public void BundledRegistry_FindIgnoresCase()
    {
        // Act && Assert
        Assert.Equal("input-group", Registry.Find("Input-Group")?.Name);
        Assert.Null(Registry.Find("frobnicator"));
    }

    private static bool Uses(string code, string type) =>
        Regex.IsMatch(code, $@"<{type}\b|(?<![\w.]){type}\.|(?<![\w.]){type}\??\s+[A-Za-z_]\w*\s*[{{;=,)]");

    private static string CodeWithoutCommentsOrStrings(RegistryComponent component)
    {
        var code = new StringBuilder();
        foreach (var file in component.Files)
        {
            var text = Encoding.UTF8.GetString(file.Content);
            text = Regex.Replace(text, @"@\*.*?\*@", "", RegexOptions.Singleline);
            text = Regex.Replace(text, @"/\*.*?\*/", "", RegexOptions.Singleline);
            text = Regex.Replace(text, @"(?m)^\s*//.*$", "");
            text = Regex.Replace(text, "\"[^\"\\n]*\"", "\"\"");
            code.Append(text);
        }

        return code.ToString();
    }
}
