// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.Extensions.FileProviders;
using Vela.UI.Demo.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Tailwind writes wwwroot/app.css outside the build, so it isn't one of the build's static
// assets; plain static files serve it. The fonts and the motion layer come straight from the
// registry, where vela init copies them from.
app.UseStaticFiles();
var registry = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "..", "src", "Core"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(registry, "Styles", "fonts")),
    RequestPath = "/fonts",
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(registry, "Scripts")),
    RequestPath = "/scripts",
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
