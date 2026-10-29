// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

// The Button page is rendered statically, so these tests listen for the browser's own click
// events: what they prove is that Button is a real <button>, and links styled as one stay links.
public class ButtonTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Button_EnterAndSpaceActivateIt()
    {
        // Arrange
        var page = await OpenAsync("button", interactive: false);
        var publish = page.GetByRole(AriaRole.Button, new() { Name = "Publish" });
        await publish.EvaluateAsync("button => { window.clicks = 0; button.addEventListener('click', () => window.clicks++); }");
        await publish.FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Enter");
        await page.Keyboard.PressAsync("Space");

        // Assert
        Assert.Equal(2, await page.EvaluateAsync<int>("() => window.clicks"));
    }

    [Fact]
    public async Task Button_DisabledCantBeFocusedOrClicked()
    {
        // Arrange
        var page = await OpenAsync("button", interactive: false);
        var saving = page.Locator("button[disabled]").First;
        await saving.EvaluateAsync("button => { window.clicks = 0; button.addEventListener('click', () => window.clicks++); }");

        // Act
        await saving.FocusAsync();
        await saving.ClickAsync(new() { Force = true });

        // Assert
        await Expect(saving).ToBeDisabledAsync();
        await Expect(saving).Not.ToBeFocusedAsync();
        Assert.Equal(0, await page.EvaluateAsync<int>("() => window.clicks"));
    }

    [Fact]
    public async Task Button_LinkStyledAsAButtonFollowsWithEnter()
    {
        // Arrange
        var page = await OpenAsync("button", interactive: false);
        await page.GetByRole(AriaRole.Link, new() { Name = "See the card examples" }).FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Enter");

        // Assert
        await Expect(page).ToHaveURLAsync(new Regex("/card$"));
    }
}
