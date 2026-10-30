// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

// The API tables are built at run time from the components' parameters and XML docs, so
// these tests check that the reflection and the XML file both reached the page.
public class DocsTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Docs_ApiTableShowsTypeDefaultAndDescription()
    {
        // Arrange
        var page = await OpenAsync("button", interactive: false);

        // Act
        var variant = page.GetByRole(AriaRole.Row).Filter(new() { HasText = "Variant" }).First;

        // Assert
        await Expect(variant).ToContainTextAsync("ButtonVariant");
        await Expect(variant).ToContainTextAsync("ButtonVariant.Default");
        await Expect(variant.GetByRole(AriaRole.Cell).Last).Not.ToBeEmptyAsync();
    }

    [Fact]
    public async Task Docs_ApiTablesCoverEveryPart()
    {
        // Arrange
        var page = await OpenAsync("dialog");

        // Act
        var headings = page.Locator("#doc-api-reference ~ div h3");

        // Assert
        await Expect(headings).ToHaveTextAsync(
            ["Dialog", "DialogTrigger", "DialogContent", "DialogHeader", "DialogTitle", "DialogDescription", "DialogFooter", "DialogClose"]);
    }

    [Theory]
    [InlineData("getting-started", "Getting started")]
    [InlineData("troubleshooting", "Troubleshooting")]
    public async Task Docs_GuidesOpen(string path, string title)
    {
        // Arrange && Act
        var page = await OpenAsync(path, interactive: false);

        // Assert
        await Expect(page.GetByRole(AriaRole.Heading, new() { Level = 1 })).ToHaveTextAsync(title);
    }

    [Fact]
    public async Task Docs_InstallLinksToThePackageAnswer()
    {
        // Arrange
        var page = await OpenAsync("label", interactive: false);

        // Act
        await page.GetByRole(AriaRole.Link, new() { Name = "Troubleshooting" }).Last.ClickAsync();

        // Assert
        await Expect(page.Locator("#package")).ToBeInViewportAsync();
    }
}
