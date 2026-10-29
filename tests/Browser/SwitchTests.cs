// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class SwitchTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Switch_IsASwitchThatSpaceTurnsOn()
    {
        // Arrange
        var page = await OpenAsync("switch");
        var airplane = page.GetByRole(AriaRole.Switch, new() { Name = "Airplane mode" });
        await Expect(airplane).Not.ToBeCheckedAsync();
        await airplane.FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Space");

        // Assert: checked is what screen readers announce as "on" for a switch.
        await Expect(airplane).ToBeCheckedAsync();
    }

    [Fact]
    public async Task Switch_TakesEffectAtOnce()
    {
        // Arrange
        var page = await OpenAsync("switch");
        await page.GetByRole(AriaRole.Switch, new() { Name = "Compact rows" }).FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Space");

        // Assert
        await Expect(page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Rows are compact.");
    }
}
