// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;

namespace Vela.UI.Browser.Tests;

/// <summary>
/// Runs the examples app on a free local port and starts Chromium once for every test in
/// the <see cref="ExamplesAppDefinition"/> collection.
/// </summary>
public sealed class ExamplesApp : IAsyncLifetime, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory = new();
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    /// <summary>
    /// Opens a page of the examples app in a fresh browser context and waits until it's
    /// interactive, so key presses reach Blazor. The caller closes the context; tests get
    /// that from <see cref="BrowserTest"/>.
    /// </summary>
    /// <param name="path">The page's path, such as <c>collapsible</c>.</param>
    /// <param name="hasTouch">Gives the page a touch screen, for testing taps.</param>
    /// <param name="interactive">
    /// Waits for the page to become interactive. Turn it off for pages rendered statically,
    /// which never do.
    /// </param>
    /// <param name="javaScriptEnabled">Runs the page with JavaScript on; off proves a page works without it.</param>
    public async Task<IPage> OpenAsync(string path, bool hasTouch = false, bool interactive = true, bool javaScriptEnabled = true)
    {
        var context = await _browser!.NewContextAsync(new()
        {
            BaseURL = _factory.ClientOptions.BaseAddress.ToString(),
            HasTouch = hasTouch,
            JavaScriptEnabled = javaScriptEnabled,
        });
        var page = await context.NewPageAsync();
        await page.GotoAsync(path);
        if (interactive && javaScriptEnabled)
        {
            await page.Locator("[data-interactive]").WaitForAsync();
        }

        return page;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        _factory.UseKestrel(0);
        _factory.StartServer();
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.DisposeAsync();
        }

        _playwright?.Dispose();
    }

    /// <inheritdoc />
    public void Dispose() => _factory.Dispose();
}
