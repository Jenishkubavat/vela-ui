// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class ToggleTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Toggle_EnterAndSpaceFlipPressed()
    {
        // Arrange
        var page = await OpenAsync("toggle");
        var bold = page.GetByRole(AriaRole.Button, new() { Name = "Bold" });
        await Expect(bold).ToHaveAttributeAsync("aria-pressed", "false");
        await bold.FocusAsync();

        // Act && Assert
        await page.Keyboard.PressAsync("Enter");
        await Expect(bold).ToHaveAttributeAsync("aria-pressed", "true");
        await page.Keyboard.PressAsync("Space");
        await Expect(bold).ToHaveAttributeAsync("aria-pressed", "false");
    }

    [Fact]
    public async Task Toggle_ChangesWhatItControls()
    {
        // Arrange
        var page = await OpenAsync("toggle");

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Italic" }).ClickAsync();

        // Assert
        await Expect(page.Locator("#preview")).ToHaveClassAsync(new Regex(@"\bitalic\b"));
    }
}
