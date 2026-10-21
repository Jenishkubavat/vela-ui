// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled <c>role="progressbar"</c> that reports how far along a task is. Put a
/// <see cref="ProgressPrimitiveIndicator"/> inside it and size the indicator from
/// <see cref="Percentage"/>.
/// </summary>
/// <remarks>
/// <para>
/// Leave <see cref="Value"/> <see langword="null"/> when the amount of work is unknown: the
/// bar becomes indeterminate and screen readers hear no value. A value outside 0 to
/// <see cref="Max"/> is treated as unknown, and a <see cref="Max"/> that isn't positive as
/// 100, as Radix does.
/// </para>
/// <para>
/// The root and indicator get <c>data-state</c> (<c>indeterminate</c>, <c>loading</c>, or
/// <c>complete</c>), <c>data-value</c>, and <c>data-max</c>. Give the bar a name with
/// <c>aria-label</c> or <c>aria-labelledby</c>. It isn't focusable and has no keyboard
/// behaviour, so it works under static server rendering.
/// </para>
/// </remarks>
public partial class ProgressPrimitive
{
    private const double DefaultMax = 100;

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
    public double Max { get; set; } = DefaultMax;

    /// <summary>
    /// Builds the text screen readers announce from the value and max, for example
    /// <c>(value, max) =&gt; $"{value} of {max} files"</c>. Defaults to a rounded percentage,
    /// such as "40%".
    /// </summary>
    [Parameter]
    public Func<double, double, string>? GetValueLabel { get; set; }

    /// <summary>
    /// The indicator, plus anything else drawn inside the bar.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c> and <c>aria-label</c>, applied to the root <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// <see cref="Max"/>, or 100 when <see cref="Max"/> isn't a positive number.
    /// </summary>
    public double CurrentMax => ValidMax(Max);

    /// <summary>
    /// <see cref="Value"/>, or <see langword="null"/> when it's outside 0 to <see cref="CurrentMax"/>.
    /// </summary>
    public double? CurrentValue => ValidValue(Value, CurrentMax);

    /// <summary>
    /// How much is done as a percentage from 0 to 100, or <see langword="null"/> when it isn't known.
    /// </summary>
    public double? Percentage => GetPercentage(Value, Max);

    /// <summary>
    /// The state for styling: <c>indeterminate</c>, <c>loading</c>, or <c>complete</c>.
    /// </summary>
    public string State => CurrentValue switch
    {
        null => "indeterminate",
        var value when value == CurrentMax => "complete",
        _ => "loading",
    };

    private string? ValueText => CurrentValue is { } value
        ? GetValueLabel?.Invoke(value, CurrentMax) ?? Format(Math.Round(value / CurrentMax * 100, MidpointRounding.AwayFromZero)) + "%"
        : null;

    /// <summary>
    /// Converts a value and max to a percentage from 0 to 100, checking them the way the bar
    /// does, so you can size an indicator before the bar has rendered. Returns
    /// <see langword="null"/> when the value isn't known or is out of range.
    /// </summary>
    public static double? GetPercentage(double? value, double max)
    {
        var validMax = ValidMax(max);
        return ValidValue(value, validMax) / validMax * 100;
    }

    private static double ValidMax(double max) => double.IsFinite(max) && max > 0 ? max : DefaultMax;

    private static double? ValidValue(double? value, double max) => value is { } number && number >= 0 && number <= max ? number : null;

    internal static string Format(double number) => number.ToString(CultureInfo.InvariantCulture);
}
