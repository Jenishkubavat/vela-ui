// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

// The page is rendered statically and these tests turn JavaScript off, so everything here
// is the browser's own form handling: the components have to be real inputs with names.
public class StaticFormTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task StaticForm_PostsCheckboxSwitchAndRadioWithoutJavaScript()
    {
        // Arrange
        var page = await OpenAsync("static-form", interactive: false, javaScriptEnabled: false);
        await page.GetByText("Send me the newsletter").ClickAsync();
        await page.Locator("#digest").FocusAsync();
        await page.Keyboard.PressAsync("Space");
        await page.GetByText("Weekly", new() { Exact = true }).ClickAsync();

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Save preferences" }).ClickAsync();

        // Assert
        await Expect(page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Saved: newsletter on, digest on, emails weekly.");
        await Expect(page.Locator("#newsletter")).ToBeCheckedAsync();
        await Expect(page.Locator("#frequency-weekly")).ToBeCheckedAsync();
    }

    [Fact]
    public async Task StaticForm_UncheckedBoxesPostAsOff()
    {
        // Arrange
        var page = await OpenAsync("static-form", interactive: false, javaScriptEnabled: false);

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Save preferences" }).ClickAsync();

        // Assert
        await Expect(page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Saved: newsletter off, digest off, emails daily.");
    }
}
