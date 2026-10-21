// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// One option in a <see cref="RadioGroupPrimitive"/>: a box with a native radio input
/// stretched invisibly over it, and a <see cref="RadioGroupPrimitiveIndicator"/> inside.
/// </summary>
/// <remarks>
/// The box gets <c>data-state</c> (<c>checked</c> or <c>unchecked</c>) and
/// <c>data-disabled</c>. <c>class</c>, <c>style</c>, and <c>data-*</c> attributes style the
/// box; everything else (<c>id</c>, <c>aria-label</c>, ...) goes on the radio input, so give
/// its <c>id</c> to your label's <c>for</c>.
/// </remarks>
public partial class RadioGroupPrimitiveItem
{
    [CascadingParameter]
    private RadioGroupPrimitive? CascadedGroup { get; set; }

    /// <summary>
    /// The value the group takes when this item is selected.
    /// </summary>
    [Parameter, EditorRequired]
    public string Value { get; set; } = "";

    /// <summary>
    /// Prevents this item from being focused or selected.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Content drawn inside the box, usually a <see cref="RadioGroupPrimitiveIndicator"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes. <c>class</c>, <c>style</c>, and <c>data-*</c> go on the box; the rest
    /// on the radio input.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Whether this item is the selected one.
    /// </summary>
    public bool IsChecked => Group.SelectedValue == Value;

    /// <summary>
    /// The item's state for styling: <c>checked</c> or <c>unchecked</c>.
    /// </summary>
    public string State => IsChecked ? "checked" : "unchecked";

    internal bool IsDisabled => Disabled || Group.Disabled;

    private RadioGroupPrimitive Group => CascadedGroup
        ?? throw new InvalidOperationException($"{nameof(RadioGroupPrimitiveItem)} must be inside a {nameof(RadioGroupPrimitive)}.");

    private Dictionary<string, object> RootAttributes
    {
        get
        {
            var (root, _) = PrimitiveAttributes.SplitForBox(AdditionalAttributes);
            root["style"] = PrimitiveAttributes.MergeStyle("position: relative", AdditionalAttributes);
            return root;
        }
    }

    private Dictionary<string, object> ControlAttributes => PrimitiveAttributes.SplitForBox(AdditionalAttributes).Control;

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Group;

    private void OnChange(ChangeEventArgs args) => Group.Select(Value);
}
