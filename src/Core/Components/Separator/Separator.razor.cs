// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A 1px hairline that divides content.
/// </summary>
/// <remarks>
/// By default it's decorative: screen readers skip it, because the content on either side
/// already explains the split. Set <see cref="Decorative"/> to <see langword="false"/> when
/// the line marks a real boundary between sections, so it's announced as a separator.
/// </remarks>
public partial class Separator
{
    private const string Classes =
        "shrink-0 bg-border " +
        "data-[orientation=horizontal]:h-px data-[orientation=horizontal]:w-full " +
        "data-[orientation=vertical]:w-px data-[orientation=vertical]:self-stretch";

    /// <summary>
    /// Which way the line runs. Defaults to <see cref="SeparatorOrientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public SeparatorOrientation Orientation { get; set; } = SeparatorOrientation.Horizontal;

    /// <summary>
    /// Hides the line from screen readers. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool Decorative { get; set; } = true;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Other attributes applied to the separator's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Uses Radix's names so selectors written for shadcn/ui work unchanged.
    private static string OrientationName(SeparatorOrientation orientation) => orientation switch
    {
        SeparatorOrientation.Horizontal => "horizontal",
        SeparatorOrientation.Vertical => "vertical",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };
}
