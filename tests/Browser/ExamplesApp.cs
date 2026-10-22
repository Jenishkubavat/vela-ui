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
    /// interactive, so key presses reach Blazor.
    /// </summary>
    /// <param name="path">The page's path, such as <c>collapsible</c>.</param>
    public async Task<IPage> OpenAsync(string path)
    {
        var context = await _browser!.NewContextAsync(new() { BaseURL = _factory.ClientOptions.BaseAddress.ToString() });
        var page = await context.NewPageAsync();
        await page.GotoAsync(path);
        await page.Locator("[data-interactive]").WaitForAsync();
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
