// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled group of <see cref="RadioGroupPrimitiveItem"/> parts, of which one can be
/// selected. Bind the selected item's value with <c>@bind-Value</c>.
/// </summary>
/// <remarks>
/// <para>
/// Each item is a native radio input sharing one <c>name</c>, so the browser provides the
/// keyboard behaviour: Tab moves into the group, and the arrow keys move between items and
/// select them. Give the group an accessible name with <c>aria-labelledby</c> or
/// <c>aria-label</c>.
/// </para>
/// <para>
/// It takes part in <see cref="EditForm"/> validation like <see cref="InputRadioGroup{TValue}"/>.
/// Without a binding it keeps its own selection.
/// </para>
/// </remarks>
public partial class RadioGroupPrimitive
{
    private readonly string _generatedName = $"radio-group-{Guid.NewGuid():N}";

    /// <summary>
    /// The layout direction, exposed as <c>aria-orientation</c> and <c>data-orientation</c>.
    /// Leave it unset when the items wrap or the direction doesn't matter.
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
    /// The items, each a <see cref="RadioGroupPrimitiveItem"/>, usually beside a label.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The value of the selected item, or <see langword="null"/> when nothing is selected.
    /// </summary>
    public string? SelectedValue => CurrentValue;

    /// <summary>
    /// The <c>name</c> every item's radio input shares: the form field name inside a
    /// server-rendered form, the <c>name</c> attribute you passed, or a generated one.
    /// </summary>
    public string GroupName => string.IsNullOrEmpty(NameAttributeValue) ? _generatedName : NameAttributeValue;

    private string? OrientationName => Orientation switch
    {
        null => null,
        Primitives.Orientation.Horizontal => "horizontal",
        Primitives.Orientation.Vertical => "vertical",
        _ => throw new ArgumentOutOfRangeException(nameof(Orientation), Orientation, null),
    };

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        // Without @bind-Value the group keeps its own selection; see CheckedInputPrimitiveBase.
        parameters.SetParameterProperties(this);
        ValueExpression ??= () => Value;
        return base.SetParametersAsync(ParameterView.Empty);
    }

    internal void Select(string value) => CurrentValue = value;

    /// <inheritdoc />
    protected override bool TryParseValueFromString(
        string? value,
        out string? result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
