// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A native <c>&lt;label&gt;</c> for naming a form control, styled with the Vela UI theme tokens.
/// </summary>
/// <remarks>
/// Clicking the label focuses or toggles its control, and screen readers announce the
/// label as the control's name. Both need <see cref="For"/> to match the control's
/// <c>id</c>, or the control nested inside the label. Double-clicking doesn't select the
/// label's text (<c>select-none</c>), which is the behavior Radix's Label primitive adds.
/// </remarks>
public partial class Label
{
    private const string LayoutClasses =
        "flex items-center gap-2 text-sm leading-none font-medium select-none";

    // The label can't see its control's disabled state, so it reads it from the markup
    // around it: either a wrapper with class="group" data-disabled="true", or a disabled
    // control with class="peer" placed before the label.
    private const string DisabledClasses =
        "group-data-[disabled=true]:pointer-events-none group-data-[disabled=true]:opacity-50 " +
        "peer-disabled:cursor-not-allowed peer-disabled:opacity-50";

    /// <summary>
    /// The <c>id</c> of the control this label names. Written as <c>for="email"</c>.
    /// </summary>
    [Parameter]
    public string? For { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The label text, optionally with an icon or a control nested inside it.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the <c>&lt;label&gt;</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
