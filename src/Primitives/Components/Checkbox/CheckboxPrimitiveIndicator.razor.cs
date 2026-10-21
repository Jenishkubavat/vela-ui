// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The tick inside a <see cref="CheckboxPrimitive"/>. It carries the checkbox's
/// <c>data-state</c> so you can show a tick when checked and a dash when indeterminate.
/// </summary>
/// <remarks>
/// Unlike Radix's indicator, it is always rendered, so a static server-rendered page can
/// reveal it with CSS (<c>:has(:checked)</c> on the box) when the person clicks.
/// </remarks>
public partial class CheckboxPrimitiveIndicator
{
    [CascadingParameter]
    private CheckboxPrimitive? Checkbox { get; set; }

    /// <summary>
    /// The tick, usually an icon.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the indicator's <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
