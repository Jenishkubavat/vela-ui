// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Playwright;

namespace Vela.UI.Browser.Tests;

/// <summary>
/// A test that opens pages of the examples app. xUnit creates one instance per test, so
/// the browser context the test opened is closed when the test ends.
/// </summary>
[Collection(ExamplesAppDefinition.Name)]
public abstract class BrowserTest(ExamplesApp app) : IAsyncLifetime
{
    private IPage? _page;

    /// <summary>
    /// Opens a page of the examples app and waits until it's interactive.
    /// </summary>
    /// <param name="path">The page's path, such as <c>collapsible</c>.</param>
    /// <param name="hasTouch">Gives the page a touch screen, for testing taps.</param>
    /// <param name="interactive">Waits for the page to become interactive; off for static pages.</param>
    /// <param name="javaScriptEnabled">Runs the page with JavaScript on; off proves a page works without it.</param>
    protected async Task<IPage> OpenAsync(string path, bool hasTouch = false, bool interactive = true, bool javaScriptEnabled = true)
    {
        _page = await app.OpenAsync(path, hasTouch, interactive, javaScriptEnabled);
        return _page;
    }

    /// <inheritdoc />
    public Task InitializeAsync() => Task.CompletedTask;

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        if (_page is not null)
        {
            await _page.Context.CloseAsync();
        }
    }
}
