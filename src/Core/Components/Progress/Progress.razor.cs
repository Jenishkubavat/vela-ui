// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// How far along something is, drawn as the wick: a 2px line with a flame at its leading
/// edge. Built on <see cref="ProgressPrimitive"/>.
/// </summary>
/// <remarks>
/// <para>
/// Give it a name with <c>aria-label</c> or <c>aria-labelledby</c>, and show the honest
/// numbers in text next to it ("4 of 10 done"). Leave <see cref="Value"/> unset when the
/// amount of work is unknown: the wick then travels along the line, or shows a still, dimmed
/// line when the person prefers reduced motion. Progress is one of the few places the flame
/// is used.
/// </para>
/// <para>
/// With <c>vela-light.js</c> loaded, an element that gets <c>data-ember</c> sends a spark to
/// the first determinate bar on the page, and its flame flares when the spark lands. It
/// renders the same under static server rendering; the bar only moves when
/// <see cref="Value"/> changes.
/// </para>
/// </remarks>
public partial class Progress
{
    // group/progress lets the indicator follow the track's data-state and data-receiving.
    // An indeterminate wick is clipped by the track and, in right-to-left layouts, mirrored
    // so it travels from the start.
    private const string TrackClasses =
        "group/progress relative h-0.5 w-full rounded-full bg-border-strong " +
        "data-[state=indeterminate]:overflow-hidden rtl:data-[state=indeterminate]:-scale-x-100";

    private const string LayoutClasses =
        "relative h-full rounded-full";

    private const string ColorClasses =
        "bg-flame forced-colors:bg-[CanvasText]";

    // The flame at the tip is the light source itself, so its glow is centred on the edge.
    private const string FlameClasses =
        "after:absolute after:-end-1 after:top-1/2 after:-mt-1 after:size-2 after:rounded-full " +
        "after:bg-flame after:shadow-[0_0_8px_1px_var(--vela-glow)]";

    private const string MotionClasses =
        "transition-[width] duration-(--vela-ember) ease-vela motion-reduce:transition-none " +
        "group-data-receiving/progress:after:animate-[vela-catch_var(--vela-ember)_var(--vela-ease)]";

    // With reduced motion the indicator keeps its full width and dims instead of travelling.
    private const string IndeterminateClasses =
        "motion-safe:group-data-[state=indeterminate]/progress:w-[30%] " +
        "motion-safe:group-data-[state=indeterminate]/progress:animate-[vela-wick_1.4s_var(--vela-ease)_infinite] " +
        "motion-reduce:group-data-[state=indeterminate]/progress:opacity-60";

    private static readonly string IndicatorClasses = ClassNames.Join(
        LayoutClasses,
        ColorClasses,
        FlameClasses,
        MotionClasses,
        IndeterminateClasses);

    /// <summary>
    /// How much is done, from 0 to <see cref="Max"/>, or <see langword="null"/> (the
    /// default) when it isn't known.
    /// </summary>
    [Parameter]
    public double? Value { get; set; }

    /// <summary>
    /// The value that means finished. Defaults to 100.
    /// </summary>
    [Parameter]
    public double Max { get; set; } = 100;

    /// <summary>
    /// Builds the text screen readers announce, for example
    /// <c>(value, max) =&gt; $"{value} of {max} done"</c>. Defaults to a rounded percentage.
    /// </summary>
    [Parameter]
    public Func<double, double, string>? GetValueLabel { get; set; }

    /// <summary>
    /// Extra CSS classes for the track, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-label</c>, applied to the track's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Width rather than shadcn's transform, so the flame at the tip isn't clipped by the track.
    private string? IndicatorStyle => ProgressPrimitive.GetPercentage(Value, Max) is { } percentage
        ? string.Create(CultureInfo.InvariantCulture, $"width: {percentage}%")
        : null;
}
