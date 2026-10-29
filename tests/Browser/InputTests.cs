// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class InputTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Input_TypingBindsTheValue()
    {
        // Arrange
        var page = await OpenAsync("input");
        await page.Locator("#name").FillAsync("Ada Lovelace");
        await page.Locator("#email").FillAsync("ada@example.com");
        await page.Locator("#password").FillAsync("correct horse");

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Create account" }).ClickAsync();

        // Assert: the form reads the bound email back.
        await Expect(page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Account created for ada@example.com.");
    }

    [Fact]
    public async Task Input_InvalidSubmitMarksAndDescribesTheField()
    {
        // Arrange
        var page = await OpenAsync("input");
        var email = page.Locator("#email");

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Create account" }).ClickAsync();

        // Assert
        await Expect(email).ToHaveAttributeAsync("aria-invalid", "true");
        await Expect(email).ToHaveAccessibleDescriptionAsync("Enter your work email.");
    }
}
