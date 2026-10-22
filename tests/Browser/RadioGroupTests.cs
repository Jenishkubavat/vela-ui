// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class RadioGroupTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task RadioGroup_TabLandsOnTheCheckedItem()
    {
        // Arrange: the last link in the navigation comes right before the first group.
        var page = await OpenAsync("radio-group");
        await page.GetByRole(AriaRole.Link, new() { Name = "Radio Group" }).FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Tab");

        // Assert: "Comfortable" is selected, so focus skips "Default".
        await Expect(page.Locator("#density-comfortable")).ToBeFocusedAsync();
    }

    [Fact]
    public async Task RadioGroup_ArrowKeysMoveAndSelect()
    {
        // Arrange
        var page = await OpenAsync("radio-group");
        await page.Locator("#density-comfortable").FocusAsync();

        // Act
        await page.Keyboard.PressAsync("ArrowDown");

        // Assert
        await Expect(page.Locator("#density-compact")).ToBeFocusedAsync();
        await Expect(page.Locator("#density-compact")).ToBeCheckedAsync();
        await Expect(page.Locator("#density-comfortable")).Not.ToBeCheckedAsync();
    }

    [Fact]
    public async Task RadioGroup_ArrowKeysUpdateTheBoundValue()
    {
        // Arrange
        var page = await OpenAsync("radio-group");
        await page.Locator("#plan-team").FocusAsync();

        // Act
        await page.Keyboard.PressAsync("ArrowUp");

        // Assert
        await Expect(page.Locator("#plan-starter")).ToBeCheckedAsync();
        await Expect(page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Selected plan: Starter");
    }
}
