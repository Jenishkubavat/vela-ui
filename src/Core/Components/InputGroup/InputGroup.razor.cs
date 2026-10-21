// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// An input or textarea with addons around it (icons, text, buttons, shortcuts) inside one
/// sunken well. Put an <see cref="InputGroupInput"/> or <see cref="InputGroupTextarea"/> in
/// it, and an <see cref="InputGroupAddon"/> for each side.
/// </summary>
/// <remarks>
/// <para>
/// The group draws the edge, focus ring, and invalid state for the control inside it. Put
/// addons before or after the control in the markup, whatever their
/// <see cref="InputGroupAddon.Align"/>, so they're read in a sensible order. Block-aligned
/// addons stack the group vertically.
/// </para>
/// <para>
/// Give the control its own name, with a <see cref="Label"/> pointing at its <c>id</c> or an
/// <c>aria-label</c>. shadcn/ui focuses the input when an addon is clicked; that needs
/// JavaScript, so it's left out here.
/// </para>
/// </remarks>
public partial class InputGroup
{
    // group/input-group lets addons react to a disabled control. Block-aligned addons turn
    // the group into a column, and the input gives up padding on the side an addon fills.
    private const string LayoutClasses =
        "group/input-group relative flex h-8 w-full min-w-0 items-center rounded-md " +
        "has-[>[data-align=block-start]]:h-auto has-[>[data-align=block-start]]:flex-col " +
        "has-[>[data-align=block-end]]:h-auto has-[>[data-align=block-end]]:flex-col has-[>textarea]:h-auto " +
        "has-[>[data-align=inline-start]]:[&>input]:ps-1.5 has-[>[data-align=inline-end]]:[&>input]:pe-1.5 " +
        "has-[>[data-align=block-start]]:[&>input]:pb-3 has-[>[data-align=block-end]]:[&>input]:pt-3";

    private const string ColorClasses =
        "bg-surface-sunken text-fg shadow-control not-has-disabled:hover:shadow-control-hover";

    private const string MotionClasses =
        "transition-shadow duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    // Focus belongs to the control, but the ring goes around the whole group.
    private const string FocusClasses =
        "has-[[data-slot=input-group-control]:focus-visible]:shadow-control-focus " +
        "has-[[data-slot=input-group-control]:focus-visible]:outline-2 " +
        "has-[[data-slot=input-group-control]:focus-visible]:outline-offset-1 " +
        "has-[[data-slot=input-group-control]:focus-visible]:outline-focus";

    private const string InvalidClasses =
        "has-[[data-slot][aria-invalid=true]]:shadow-control-invalid";

    private const string DisabledClasses =
        "has-disabled:bg-transparent has-disabled:shadow-hairline";

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The control and its addons.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the group's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
