// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled line that divides content. Style it through <c>data-orientation</c>
/// (<c>horizontal</c> or <c>vertical</c>).
/// </summary>
/// <remarks>
/// By default it's decorative (<c>role="none"</c>), so screen readers skip it. Set
/// <see cref="Decorative"/> to <see langword="false"/> when it marks a real boundary
/// between sections: it then gets <c>role="separator"</c>, plus
/// <c>aria-orientation="vertical"</c> when vertical (horizontal is the implied default).
/// </remarks>
public partial class SeparatorPrimitive
{
    /// <summary>
    /// Which way the line runs. Defaults to <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Hides the line from screen readers. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Decorative { get; set; } = true;

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the separator's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Radix's names, so selectors written for Radix and shadcn/ui work unchanged.
    private static string OrientationName(Orientation orientation) => orientation switch
    {
        Orientation.Horizontal => "horizontal",
        Orientation.Vertical => "vertical",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };
}
