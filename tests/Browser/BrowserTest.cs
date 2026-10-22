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
    protected async Task<IPage> OpenAsync(string path)
    {
        _page = await app.OpenAsync(path);
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
