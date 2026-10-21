// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Primitives;

/// <summary>
/// The shared behaviour of <see cref="CheckboxPrimitive"/> and <see cref="SwitchPrimitive"/>:
/// a visible box with a native checkbox stretched invisibly over it.
/// </summary>
/// <remarks>
/// <para>
/// The native input keeps keyboard support (Space), focus, label clicks, screen reader
/// semantics, and form posts working without script. Style the box from its
/// <c>data-state</c>, or from <c>:has(:checked)</c>, which also follows clicks in static
/// server-rendered forms where nothing re-renders.
/// </para>
/// <para>
/// Bind it with <c>@bind-Value</c>. It takes part in <see cref="EditForm"/> validation like
/// <see cref="InputCheckbox"/>. Without a binding it keeps its own state.
/// </para>
/// </remarks>
public abstract class CheckedInputPrimitiveBase : InputBase<bool>
{
    /// <summary>
    /// Stretches the native input over the box, invisible but still clickable and focusable.
    /// </summary>
    protected const string HiddenInputStyle =
        "position: absolute; inset: 0; width: 100%; height: 100%; margin: 0; opacity: 0; cursor: inherit";

    /// <summary>
    /// Prevents the control from being focused or changed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Content drawn inside the box, such as an indicator or thumb part.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The control's state for styling: <c>checked</c> or <c>unchecked</c>.
    /// </summary>
    public virtual string State => CurrentValue ? "checked" : "unchecked";

    /// <summary>
    /// Attributes for the visible box: <c>class</c>, <c>data-*</c>, and the merged <c>style</c>.
    /// </summary>
    protected Dictionary<string, object> RootAttributes
    {
        get
        {
            var (root, _) = PrimitiveAttributes.SplitForBox(AdditionalAttributes);
            root["style"] = PrimitiveAttributes.MergeStyle("position: relative", AdditionalAttributes);
            return root;
        }
    }

    /// <summary>
    /// Attributes for the native input, including the <c>aria-invalid</c> that
    /// <see cref="InputBase{TValue}"/> adds when the bound field has validation errors.
    /// </summary>
    protected Dictionary<string, object> ControlAttributes => PrimitiveAttributes.SplitForBox(AdditionalAttributes).Control;

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        // InputBase requires a ValueExpression, which @bind-Value supplies. Without a binding the
        // control keeps its own state, so point the expression at its own Value instead. The
        // parameters are applied first so an explicitly passed null is replaced too.
        parameters.SetParameterProperties(this);
        ValueExpression ??= () => Value;
        return base.SetParametersAsync(ParameterView.Empty);
    }

    /// <summary>
    /// Updates the value from the native input's change event.
    /// </summary>
    protected void OnChange(ChangeEventArgs args) =>
        CurrentValue = args.Value is bool value ? value : bool.TryParse(args.Value?.ToString(), out var parsed) && parsed;

    /// <inheritdoc />
    protected override bool TryParseValueFromString(
        string? value,
        out bool result,
        [NotNullWhen(false)] out string? validationErrorMessage) =>
        throw new NotSupportedException($"{GetType().Name} sets its value directly and doesn't parse strings.");
}
