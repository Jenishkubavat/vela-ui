// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

[Collection(ExamplesAppDefinition.Name)]
public class CollapsibleTests(ExamplesApp app)
{
    [Fact]
    public async Task Collapsible_EnterOpensIt()
    {
        // Arrange
        var page = await app.OpenAsync("collapsible");
        var trigger = page.GetByRole(AriaRole.Button, new() { Name = "Show all repositories" });
        await trigger.FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Enter");

        // Assert
        await Expect(trigger).ToHaveAttributeAsync("aria-expanded", "true");
        await Expect(page.GetByText("blazor/forms")).ToBeVisibleAsync();
        await Expect(trigger).ToBeFocusedAsync();
    }

    [Fact]
    public async Task Collapsible_SpaceClosesIt()
    {
        // Arrange
        var page = await app.OpenAsync("collapsible");
        var trigger = page.GetByRole(AriaRole.Button, new() { Name = "Show all repositories" });
        await trigger.FocusAsync();
        await page.Keyboard.PressAsync("Enter");
        await Expect(trigger).ToHaveAttributeAsync("aria-expanded", "true");

        // Act
        await page.Keyboard.PressAsync("Space");

        // Assert
        await Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false");
        await Expect(page.GetByText("blazor/forms")).ToBeHiddenAsync();
        await Expect(page.GetByText("Showing 1 of 3.")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Collapsible_TriggerPointsToTheContent()
    {
        // Arrange
        var page = await app.OpenAsync("collapsible");
        var trigger = page.GetByRole(AriaRole.Button, new() { Name = "Show all repositories" });

        // Act
        await trigger.ClickAsync();

        // Assert
        var contentId = await trigger.GetAttributeAsync("aria-controls");
        await Expect(page.Locator($"#{contentId}")).ToContainTextAsync("blazor/routing");
    }
}
