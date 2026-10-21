// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// A set of <see cref="RadioGroupItem"/> options of which one can be selected, styled on top
/// of <see cref="RadioGroupPrimitive"/>. Bind the selected value with <c>@bind-Value</c>.
/// </summary>
/// <remarks>
/// The items are native radio inputs, so Tab moves into the group and the arrow keys move
/// between items. Name the group with <c>aria-labelledby</c>, or put it in a
/// <c>FieldSet</c> with a <c>FieldLegend</c>.
/// </remarks>
public partial class RadioGroup
{
    private const string Classes = "grid w-full gap-2";

    /// <summary>
    /// The selected item's value. Use <c>@bind-Value</c> to keep it in sync.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Invoked with the new value when the person selects an item.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// Identifies the bound field for <c>EditForm</c> validation. <c>@bind-Value</c> sets it.
    /// </summary>
    [Parameter]
    public Expression<Func<string?>>? ValueExpression { get; set; }

    /// <summary>
    /// The layout direction, announced to screen readers. Leave it unset when items wrap.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Prevents every item from being focused or selected.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Requires a selection before the form can be submitted.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults. Use <c>flex</c> for a horizontal row.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The items, each a <see cref="RadioGroupItem"/> beside a label.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-labelledby</c>, applied to the group's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Keeps this component's own Value in step, so it doesn't reset the primitive when its
    // parent re-renders a group that isn't bound.
    private async Task OnValueChanged(string? value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
