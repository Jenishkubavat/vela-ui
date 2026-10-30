// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Demo.Components.Docs;

/// <summary>
/// Every page of the site, in the order the navigation and the home page list them.
/// </summary>
internal static class SitePages
{
    /// <summary>
    /// Guides that aren't about one component.
    /// </summary>
    public static readonly SitePage[] Guides =
    [
        new("getting-started", "Getting started", "Set up a Blazor app, build the CSS, and add your first component."),
        new("troubleshooting", "Troubleshooting", "Package restore, missing styles, render modes, and other common problems."),
    ];

    /// <summary>
    /// One page per component, then the static form.
    /// </summary>
    public static readonly SitePage[] Components =
    [
        new("button", "Button", "Variants and sizes, a loading state, and a link styled as a button."),
        new("input", "Input", "A text field on its own, and a sign-up form with validation."),
        new("label", "Label", "Labels for a checkbox and for switches in a settings list."),
        new("card", "Card", "A plain card, and a form inside a card with actions."),
        new("dialog", "Dialog", "A form, a confirmation with Ctrl+Enter, and a dialog that scrolls."),
        new("collapsible", "Collapsible", "A section that opens and closes, with Enter and Space."),
        new("radio-group", "Radio Group", "Options moved through with the arrow keys, and a plan picker."),
        new("checkbox", "Checkbox", "A single checkbox, and a select-all box that shows a dash for a partial selection."),
        new("switch", "Switch", "A setting that takes effect at once, announced as on or off."),
        new("toggle", "Toggle", "A text toolbar whose toggles change a preview."),
        new("avatar", "Avatar", "Photos with initials underneath, a photo that fails, and the current-user ring."),
        new("static-form", "Static form", "Checkbox, Switch, and Radio Group posted with no interactivity."),
    ];
}

/// <summary>
/// A page of the site: its path, its name in the navigation, and a line about it.
/// </summary>
internal sealed record SitePage(string Href, string Name, string Description);
