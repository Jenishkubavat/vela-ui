// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// Joins related buttons, inputs, and text into one control: the inner corners square off and
/// neighbouring edges merge into a single line.
/// </summary>
/// <remarks>
/// <para>
/// It renders <c>role="group"</c>; give it an <c>aria-label</c> ("Text alignment") so screen
/// readers announce what the buttons have in common. Tab still moves through each button.
/// Nest groups to space sets apart, and divide buttons that have no edge of their own
/// (<see cref="ButtonVariant.Secondary"/>, <see cref="ButtonVariant.Ghost"/>) with a
/// <see cref="ButtonGroupSeparator"/>.
/// </para>
/// <para>
/// To group things that aren't buttons, such as a toolbar of toggles, prefer the component
/// made for it. Use <see cref="GetClasses"/> to style another element as a group.
/// </para>
/// </remarks>
public partial class ButtonGroup
{
    // A focused child is lifted so its outline isn't covered by the next one.
    private const string LayoutClasses =
        "group/button-group flex w-fit items-stretch *:focus-visible:relative *:focus-visible:z-10 " +
        "has-[>[data-slot=button-group]]:gap-2 [&>input]:flex-1";

    // Neighbours overlap by 1px so their two edges draw one line. Separators and nested groups
    // keep their own space.
    private const string HorizontalClasses =
        "[&>*:not(:first-child)]:rounded-s-none [&>*:not(:last-child)]:rounded-e-none " +
        "[&>:not([data-slot=button-group-separator])+:not([data-slot=button-group-separator],[data-slot=button-group])]:-ms-px";

    private const string VerticalClasses =
        "flex-col [&>*:not(:first-child)]:rounded-t-none [&>*:not(:last-child)]:rounded-b-none " +
        "[&>:not([data-slot=button-group-separator])+:not([data-slot=button-group-separator],[data-slot=button-group])]:-mt-px";

    /// <summary>
    /// Whether the items sit side by side or stack. Defaults to <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The buttons, inputs, text, separators, or nested groups.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-label</c>, applied to the group's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes a <see cref="ButtonGroup"/> with this orientation would use, so
    /// another element can join its children the same way. This is the Blazor counterpart of
    /// shadcn/ui's <c>buttonGroupVariants</c>.
    /// </summary>
    public static string GetClasses(Orientation orientation = Orientation.Horizontal, string? additionalClasses = null) =>
        ClassNames.Join(LayoutClasses, OrientationClasses(orientation), additionalClasses);

    private static string OrientationClasses(Orientation orientation) => orientation switch
    {
        Orientation.Horizontal => HorizontalClasses,
        Orientation.Vertical => VerticalClasses,
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };

    private static string OrientationName(Orientation orientation) => orientation switch
    {
        Orientation.Horizontal => "horizontal",
        Orientation.Vertical => "vertical",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };
}
