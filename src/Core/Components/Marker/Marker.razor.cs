// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A short, quiet line of context in a conversation or feed, such as "Today",
/// "Ada joined", or "Earlier messages", built from an optional <see cref="MarkerIcon"/>
/// and <see cref="MarkerContent"/>.
/// </summary>
public partial class Marker
{
    private const string LayoutClasses =
        "group/marker relative flex min-h-4 w-full items-center gap-2 text-start text-body-sm text-fg-muted " +
        "[&_svg:not([class*='size-'])]:size-4 [a]:underline [a]:underline-offset-[0.2em] [a]:hover:text-fg";

    /// <summary>
    /// The style. Defaults to <see cref="MarkerVariant.Default"/>.
    /// </summary>
    [Parameter]
    public MarkerVariant Variant { get; set; } = MarkerVariant.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// A <see cref="MarkerIcon"/> and <see cref="MarkerContent"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the marker's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes a <see cref="Marker"/> with this variant uses, so a link can look
    /// like one.
    /// </summary>
    public static string GetClasses(MarkerVariant variant = MarkerVariant.Default, string? additionalClasses = null) =>
        ClassNames.Join(LayoutClasses, VariantClasses(variant), additionalClasses);

    // The separator lines are pseudo-elements, so they need no extra markup.
    private static string VariantClasses(MarkerVariant variant) => variant switch
    {
        MarkerVariant.Default => "",
        MarkerVariant.Separator =>
            "before:me-1 before:h-px before:min-w-0 before:flex-1 before:bg-border " +
            "after:ms-1 after:h-px after:min-w-0 after:flex-1 after:bg-border",
        MarkerVariant.Border => "border-b pb-2",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string VariantName(MarkerVariant variant) => variant switch
    {
        MarkerVariant.Default => "default",
        MarkerVariant.Separator => "separator",
        MarkerVariant.Border => "border",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
