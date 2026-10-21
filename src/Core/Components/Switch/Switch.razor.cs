// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// An on/off switch styled on top of <see cref="SwitchPrimitive"/>, for a setting that takes
/// effect immediately. Use a <see cref="Checkbox"/> when the choice waits for a Save button.
/// </summary>
/// <remarks>
/// Bind it with <c>@bind-Value</c>. When on, the track fills with the text colour and the
/// thumb slides over in 240ms. Name the setting, not the action, in its label
/// ("Daily summary email"). <c>class</c> and <c>data-*</c> attributes style the track; the
/// rest (<c>id</c>, <c>aria-*</c>) go on the native input.
/// </remarks>
public partial class Switch
{
    // The size sets two variables that the track and thumb share.
    private const string LayoutClasses =
        "group/switch relative inline-flex shrink-0 cursor-pointer items-center rounded-full " +
        "data-[size=default]:h-[1.125rem] data-[size=default]:w-8 " +
        "data-[size=default]:[--switch-thumb:0.75rem] data-[size=default]:[--switch-travel:0.875rem] " +
        "data-[size=sm]:h-3.5 data-[size=sm]:w-6 " +
        "data-[size=sm]:[--switch-thumb:0.5rem] data-[size=sm]:[--switch-travel:0.625rem]";

    private const string ColorClasses =
        "bg-surface-sunken shadow-check has-checked:bg-fg has-checked:shadow-none " +
        "hover:not-has-checked:not-has-disabled:shadow-check-hover";

    private const string MotionClasses =
        "transition-[background-color,box-shadow] duration-(--vela-settle) ease-vela motion-reduce:transition-none";

    // Focus lands on the hidden native input, so the track draws the ring for it.
    private const string FocusClasses =
        "has-[input:focus-visible]:outline-2 has-[input:focus-visible]:outline-offset-2 " +
        "has-[input:focus-visible]:outline-focus";

    private const string InvalidClasses = "has-aria-invalid:shadow-check-invalid";

    private const string DisabledClasses = "has-disabled:cursor-not-allowed has-disabled:opacity-50";

    // The thumb slides by --switch-travel, the other way in right-to-left layouts.
    private const string ThumbClasses =
        "pointer-events-none absolute start-[3px] top-1/2 size-(--switch-thumb) -translate-y-1/2 rounded-full bg-fg-muted " +
        "transition-[translate,background-color] duration-(--vela-settle) ease-vela motion-reduce:transition-none " +
        "group-has-checked/switch:translate-x-(--switch-travel) group-has-checked/switch:bg-surface " +
        "rtl:group-has-checked/switch:-translate-x-(--switch-travel)";

    /// <summary>
    /// Whether the switch is on. Use <c>@bind-Value</c> to keep it in sync.
    /// </summary>
    [Parameter]
    public bool Value { get; set; }

    /// <summary>
    /// Invoked with the new value when the person toggles the switch.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    /// <summary>
    /// Identifies the bound field for <c>EditForm</c> validation. <c>@bind-Value</c> sets it.
    /// </summary>
    [Parameter]
    public Expression<Func<bool>>? ValueExpression { get; set; }

    /// <summary>
    /// The track size. Defaults to <see cref="SwitchSize.Default"/>.
    /// </summary>
    [Parameter]
    public SwitchSize Size { get; set; } = SwitchSize.Default;

    /// <summary>
    /// Prevents the switch from being focused or changed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Extra CSS classes for the track, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Other attributes. <c>data-*</c> attributes go on the track; the rest on the native input.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Keeps this component's own Value in step, so it doesn't reset the primitive when its
    // parent re-renders a switch that isn't bound.
    private async Task OnValueChanged(bool value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
