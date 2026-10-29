// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class AvatarTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Avatar_BrokenPhotoLeavesTheInitials()
    {
        // Arrange && Act
        var page = await OpenAsync("avatar");
        var grace = page.Locator("#owner-grace");

        // Assert: the failed image is removed and the initials stay.
        await Expect(grace.Locator("img")).ToHaveCountAsync(0);
        await Expect(grace.Locator("[data-slot=avatar-fallback]")).ToHaveTextAsync("GH");
    }

    [Fact]
    public async Task Avatar_LoadedPhotoReplacesTheInitials()
    {
        // Arrange && Act
        var page = await OpenAsync("avatar");
        var ada = page.Locator("#owner-ada");

        // Assert
        await Expect(ada.Locator("img")).ToBeVisibleAsync();
        await Expect(ada.Locator("[data-slot=avatar-fallback]")).ToHaveCountAsync(0);
    }
}
