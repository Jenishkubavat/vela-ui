// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class CheckboxTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Checkbox_SpaceTogglesIt()
    {
        // Arrange
        var page = await OpenAsync("checkbox");
        await page.Locator("#terms").FocusAsync();

        // Act
        await page.Keyboard.PressAsync("Space");

        // Assert
        await Expect(page.Locator("#terms")).ToBeCheckedAsync();
        await Expect(page.Locator("#terms-state")).ToHaveTextAsync("Terms accepted.");
    }

    [Fact]
    public async Task Checkbox_ClickingItsLabelTogglesIt()
    {
        // Arrange
        var page = await OpenAsync("checkbox");

        // Act
        await page.GetByText("Accept the terms").ClickAsync();

        // Assert
        await Expect(page.Locator("#terms")).ToBeCheckedAsync();
        await Expect(page.Locator("#terms-state")).ToHaveTextAsync("Terms accepted.");
    }

    [Fact]
    public async Task Checkbox_PartlyCheckedListShowsMixedUntilAllAreChecked()
    {
        // Arrange: one of three tasks starts done.
        var page = await OpenAsync("checkbox");
        var all = page.Locator("#all-tasks");
        await Expect(all).ToHaveAttributeAsync("aria-checked", "mixed");

        // Act
        await all.ClickAsync();

        // Assert
        await Expect(page.Locator("#tasks-state")).ToHaveTextAsync("3 of 3 done");
        await Expect(all).ToBeCheckedAsync();
        await Expect(all).Not.ToHaveAttributeAsync("aria-checked", "mixed");
    }
}
