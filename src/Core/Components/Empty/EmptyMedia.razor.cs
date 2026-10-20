// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// The icon or image above an <see cref="EmptyTitle"/>. It's decorative and hidden from
/// screen readers; the title and description carry the meaning.
/// </summary>
public partial class EmptyMedia
{
    private const string LayoutClasses =
        "mb-2 flex shrink-0 items-center justify-center [&_svg]:pointer-events-none [&_svg]:shrink-0";

    /// <summary>
    /// How the media is framed. Defaults to <see cref="EmptyMediaVariant.Default"/>.
    /// </summary>
    [Parameter]
    public EmptyMediaVariant Variant { get; set; } = EmptyMediaVariant.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// An icon or image.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the media's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private static string VariantClasses(EmptyMediaVariant variant) => variant switch
    {
        EmptyMediaVariant.Default => "bg-transparent",
        EmptyMediaVariant.Icon =>
            "size-8 rounded-lg bg-surface-raised text-fg shadow-hairline [&_svg:not([class*='size-'])]:size-4",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string VariantName(EmptyMediaVariant variant) => variant switch
    {
        EmptyMediaVariant.Default => "default",
        EmptyMediaVariant.Icon => "icon",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
