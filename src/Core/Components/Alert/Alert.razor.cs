// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A message that stays on the page until it's no longer true, built from an optional
/// icon, <see cref="AlertTitle"/>, <see cref="AlertDescription"/>, and <see cref="AlertAction"/>.
/// </summary>
/// <remarks>
/// <para>
/// End the description with the way forward ("Add one and publish again"). For something
/// that just happened, use a toast instead.
/// </para>
/// <para>
/// An alert that's on the page when it loads needs no role. When you insert one in
/// response to something the person did, add <c>role="alert"</c> (urgent) or
/// <c>role="status"</c> (polite) so screen readers announce it. shadcn/ui always sets
/// <c>role="alert"</c>; Vela doesn't, so static alerts don't interrupt.
/// </para>
/// </remarks>
public partial class Alert
{
    // An icon placed first gets its own column; the title and description line up beside it.
    private const string LayoutClasses =
        "group/alert relative grid w-full gap-0.5 rounded-lg px-3.5 py-3 text-start text-body-sm text-fg " +
        "has-data-[slot=alert-action]:pe-18 has-[>svg]:grid-cols-[auto_1fr] has-[>svg]:gap-x-3";

    private const string IconClasses =
        "*:[svg]:row-span-2 *:[svg]:translate-y-0.5 *:[svg:not([class*='size-'])]:size-4";

    /// <summary>
    /// The visual style. Defaults to <see cref="AlertVariant.Default"/>.
    /// </summary>
    [Parameter]
    public AlertVariant Variant { get; set; } = AlertVariant.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// An optional icon, then the title, description, and action.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>role</c>, applied to the alert's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Only the icon takes colour; Destructive also gets a red edge.
    private static string VariantClasses(AlertVariant variant) => variant switch
    {
        AlertVariant.Default => "bg-surface shadow-hairline *:[svg]:text-fg-muted",
        AlertVariant.Destructive => "bg-surface shadow-control-invalid *:[svg]:text-danger",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string VariantName(AlertVariant variant) => variant switch
    {
        AlertVariant.Default => "default",
        AlertVariant.Destructive => "destructive",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
