// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Vela.UI.Browser.Tests;

public class DialogTests(ExamplesApp app) : BrowserTest(app)
{
    [Fact]
    public async Task Dialog_FocusMovesToTheFirstField()
    {
        // Arrange
        var page = await OpenAsync("dialog");

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" }).ClickAsync();

        // Assert: the first focusable element, not the close button, which comes last.
        await Expect(page.GetByRole(AriaRole.Dialog, new() { Name = "Edit profile" })).ToBeVisibleAsync();
        await Expect(page.Locator("#profile-name")).ToBeFocusedAsync();
    }

    [Fact]
    public async Task Dialog_TabNeverReachesThePageBehind()
    {
        // Arrange
        var page = await OpenAsync("dialog");
        await page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" }).ClickAsync();
        await Expect(page.Locator("#profile-name")).ToBeFocusedAsync();

        for (var press = 0; press < 6; press++)
        {
            // Act
            await page.Keyboard.PressAsync("Tab");

            // Assert: focus is in the dialog, or has left the page for the browser's own UI.
            var outside = await page.EvaluateAsync<bool>(
                "() => { const el = document.activeElement; return !!el && el !== document.body && !el.closest('dialog'); }");
            Assert.False(outside, $"Tab {press + 1} moved focus to the page behind the dialog.");
        }
    }

    [Fact]
    public async Task Dialog_EscapeClosesAndReturnsFocusToTheTrigger()
    {
        // Arrange
        var page = await OpenAsync("dialog");
        var trigger = page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" });
        await trigger.FocusAsync();
        await page.Keyboard.PressAsync("Enter");
        await Expect(page.Locator("#profile-name")).ToBeFocusedAsync();

        // Act
        await page.Keyboard.PressAsync("Escape");

        // Assert
        await Expect(page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync(0);
        await Expect(trigger).ToBeFocusedAsync();
        await Expect(trigger).ToHaveAttributeAsync("aria-expanded", "false");
    }

    [Fact]
    public async Task Dialog_BackdropClickClosesWithoutClickingThePage()
    {
        // Arrange: a page button sits under the backdrop once the dialog is open.
        var page = await OpenAsync("dialog");
        var pageButton = await page.GetByRole(AriaRole.Button, new() { Name = "Page button" }).BoundingBoxAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" }).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync();

        // Act
        await page.Mouse.ClickAsync(pageButton!.X + pageButton.Width / 2, pageButton.Y + pageButton.Height / 2);

        // Assert
        await Expect(page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync(0);
        await Expect(page.Locator("#page-clicks")).ToHaveTextAsync("Clicked 0 times");
    }

    [Fact]
    public async Task Dialog_BackdropPressWaitsForTheClickToClose()
    {
        // Arrange: closing on the press, not the click, is what lets a click fall through to
        // the page in some browsers, so the dialog must still be open while the button is down.
        var page = await OpenAsync("dialog");
        await page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" }).ClickAsync();
        var dialog = page.GetByRole(AriaRole.Dialog);
        await Expect(dialog).ToBeVisibleAsync();
        await page.Mouse.MoveAsync(5, 5);

        // Act && Assert
        await page.Mouse.DownAsync();
        await page.WaitForTimeoutAsync(200);
        await Expect(dialog).ToBeVisibleAsync();
        await page.Mouse.UpAsync();
        await Expect(dialog).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task Dialog_BackdropTapClosesWithoutTappingThePage()
    {
        // Arrange: on touch, a dialog that closed at the start of the tap would let the tap's
        // click land on the button that's now under the finger.
        var page = await OpenAsync("dialog", hasTouch: true);
        var pageButton = await page.GetByRole(AriaRole.Button, new() { Name = "Page button" }).BoundingBoxAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" }).TapAsync();
        await Expect(page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync();

        // Act
        await page.Touchscreen.TapAsync(pageButton!.X + pageButton.Width / 2, pageButton.Y + pageButton.Height / 2);

        // Assert
        await Expect(page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync(0);
        await Expect(page.Locator("#page-clicks")).ToHaveTextAsync("Clicked 0 times");
    }

    [Fact]
    public async Task Dialog_DragFromInsideToOutsideStaysOpen()
    {
        // Arrange
        var page = await OpenAsync("dialog");
        await page.GetByRole(AriaRole.Button, new() { Name = "Edit profile" }).ClickAsync();
        var description = page.GetByText("Change how your name appears");
        await Expect(description).ToBeVisibleAsync();
        var text = await description.BoundingBoxAsync();

        // Act: select text and let go over the backdrop.
        await page.Mouse.MoveAsync(text!.X + 2, text.Y + text.Height / 2);
        await page.Mouse.DownAsync();
        await page.Mouse.MoveAsync(5, 5, new() { Steps = 5 });
        await page.Mouse.UpAsync();

        // Assert
        await Expect(page.GetByRole(AriaRole.Dialog, new() { Name = "Edit profile" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Dialog_PressOnItsScrollbarStaysOpen()
    {
        // Arrange
        var page = await OpenAsync("dialog");
        await page.GetByRole(AriaRole.Button, new() { Name = "Read the terms" }).ClickAsync();
        var dialog = page.GetByRole(AriaRole.Dialog, new() { Name = "Terms of service" });
        await Expect(dialog).ToBeVisibleAsync();
        Assert.True(await dialog.EvaluateAsync<bool>("d => d.scrollHeight > d.clientHeight"), "The dialog should be tall enough to scroll.");
        var box = await dialog.BoundingBoxAsync();

        // Act: the scrollbar, or where it would be when the browser hides it, at the right edge.
        await page.Mouse.ClickAsync(box!.X + box.Width - 3, box.Y + box.Height / 2);

        // Assert
        await Expect(dialog).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Dialog_IsNamedByItsTitleAndDescribedByItsDescription()
    {
        // Arrange
        var page = await OpenAsync("dialog");

        // Act
        await page.GetByRole(AriaRole.Button, new() { Name = "Archive projects" }).ClickAsync();

        // Assert
        var dialog = page.GetByRole(AriaRole.Dialog, new() { Name = "Archive 3 projects?" });
        await Expect(dialog).ToBeVisibleAsync();
        await Expect(dialog).ToHaveAccessibleDescriptionAsync(new System.Text.RegularExpressions.Regex("restore them from Settings"));
    }

    [Fact]
    public async Task Dialog_CtrlEnterConfirms()
    {
        // Arrange
        var page = await OpenAsync("dialog");
        await page.GetByRole(AriaRole.Button, new() { Name = "Archive projects" }).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Dialog)).ToBeVisibleAsync();

        // Act
        await page.Keyboard.PressAsync("Control+Enter");

        // Assert
        await Expect(page.GetByRole(AriaRole.Status)).ToHaveTextAsync("Archived 3 projects.");
        await Expect(page.GetByRole(AriaRole.Dialog)).ToHaveCountAsync(0);
    }
}
