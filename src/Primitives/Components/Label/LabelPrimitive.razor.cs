// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled native <c>&lt;label&gt;</c> that names a form control.
/// </summary>
/// <remarks>
/// Clicking the label focuses or toggles its control, and screen readers announce the
/// label as the control's name. Both need <see cref="For"/> to match the control's
/// <c>id</c>, or the control nested inside the label. Radix's Label also stops a
/// double-click from selecting the label's text; in Blazor that needs CSS, so add
/// <c>user-select: none</c> in your own styles if you want it.
/// </remarks>
public partial class LabelPrimitive
{
    /// <summary>
    /// The <c>id</c> of the control this label names. Written as <c>for="email"</c>.
    /// </summary>
    [Parameter]
    public string? For { get; set; }

    /// <summary>
    /// The label text, optionally with a control nested inside it.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the <c>&lt;label&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
