// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The dot inside a <see cref="RadioGroupPrimitiveItem"/>. It carries the item's
/// <c>data-state</c>, and like <see cref="CheckboxPrimitiveIndicator"/> it is always rendered
/// so CSS can reveal it in static server-rendered forms.
/// </summary>
public partial class RadioGroupPrimitiveIndicator
{
    [CascadingParameter]
    private RadioGroupPrimitiveItem? Item { get; set; }

    /// <summary>
    /// The dot, if you draw it with markup rather than CSS.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the indicator's <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
