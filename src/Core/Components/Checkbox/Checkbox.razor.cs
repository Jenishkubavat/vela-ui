// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// A checkbox styled on top of <see cref="CheckboxPrimitive"/>: a small sunken square that
/// fills with the text colour when checked.
/// </summary>
/// <remarks>
/// <para>
/// Bind it with <c>@bind-Value</c>; it takes part in <c>EditForm</c> validation. Without a
/// binding it keeps its own state. Space toggles it.
/// </para>
/// <para>
/// <c>class</c> and <c>data-*</c> attributes style the box; every other attribute goes on
/// the native checkbox, so pair it with a label like this:
/// <c>&lt;Checkbox id="terms" @bind-Value="accepted" /&gt;&lt;Label For="terms"&gt;Accept terms&lt;/Label&gt;</c>.
/// The visuals follow the native checked state, so they also work in static
/// server-rendered forms.
/// </para>
/// </remarks>
public partial class Checkbox
{
    private const string LayoutClasses =
        "group/checkbox peer relative inline-grid size-4 shrink-0 cursor-pointer place-items-center rounded-sm";

    // Checked fills with the text colour, not the flame: it's a state, not an action.
    private const string ColorClasses =
        "bg-surface-sunken text-surface shadow-check has-checked:bg-fg has-checked:shadow-none " +
        "data-[state=indeterminate]:bg-fg data-[state=indeterminate]:shadow-none " +
        "hover:not-has-checked:not-has-disabled:shadow-check-hover";

    private const string MotionClasses =
        "transition-[background-color,box-shadow] duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    // Focus lands on the hidden native input, so the box draws the ring for it.
    private const string FocusClasses =
        "has-[input:focus-visible]:outline-2 has-[input:focus-visible]:outline-offset-2 " +
        "has-[input:focus-visible]:outline-focus";

    private const string InvalidClasses = "has-aria-invalid:shadow-check-invalid";

    private const string DisabledClasses = "has-disabled:cursor-not-allowed has-disabled:opacity-50";

    private const string IndicatorClasses =
        "pointer-events-none grid place-items-center invisible " +
        "group-has-checked/checkbox:visible group-data-[state=indeterminate]/checkbox:visible";

    private const string TickClasses =
        "size-3 group-data-[state=indeterminate]/checkbox:hidden " +
        "group-has-checked/checkbox:animate-[vela-tick_var(--vela-settle)_var(--vela-ease)_both] motion-reduce:animate-none";

    private const string DashClasses = "hidden size-3 group-data-[state=indeterminate]/checkbox:block";

    /// <summary>
    /// Whether the checkbox is checked. Use <c>@bind-Value</c> to keep it in sync.
    /// </summary>
    [Parameter]
    public bool Value { get; set; }

    /// <summary>
    /// Invoked with the new value when the person toggles the checkbox.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    /// <summary>
    /// Identifies the bound field for <c>EditForm</c> validation. <c>@bind-Value</c> sets it.
    /// </summary>
    [Parameter]
    public Expression<Func<bool>>? ValueExpression { get; set; }

    /// <summary>
    /// Shows a dash for a "select all" box whose items are only partly selected. Clicking it
    /// checks it; recompute this from the items afterwards.
    /// </summary>
    [Parameter]
    public bool Indeterminate { get; set; }

    /// <summary>
    /// Prevents the checkbox from being focused or changed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Extra CSS classes for the box, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Other attributes. <c>data-*</c> attributes go on the box; the rest (<c>id</c>,
    /// <c>name</c>, <c>aria-*</c>, <c>required</c>) on the native checkbox.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Keeps this component's own Value in step, so it doesn't reset the primitive when its
    // parent re-renders a checkbox that isn't bound.
    private async Task OnValueChanged(bool value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
