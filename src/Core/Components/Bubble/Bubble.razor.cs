// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A chat bubble: a <see cref="BubbleContent"/> with an optional <see cref="BubbleReactions"/>.
/// </summary>
/// <remarks>
/// The variant colours the <see cref="BubbleContent"/> inside, so one bubble can switch
/// style without touching its content. Inside an end-aligned <see cref="Message"/> the
/// bubble moves to the end side too. Bubbles are capped at 80% of the row so long
/// messages stay readable; <see cref="BubbleVariant.Ghost"/> uses the full width.
/// </remarks>
public partial class Bubble
{
    private const string LayoutClasses =
        "group/bubble relative flex w-fit max-w-[80%] min-w-0 flex-col gap-1 " +
        "group-data-[align=end]/message:self-end data-[align=end]:self-end data-[variant=ghost]:max-w-full";

    /// <summary>
    /// The fill. Defaults to <see cref="BubbleVariant.Default"/>.
    /// </summary>
    [Parameter]
    public BubbleVariant Variant { get; set; } = BubbleVariant.Default;

    /// <summary>
    /// Which side the bubble sits on. Defaults to <see cref="BubbleAlign.Start"/>.
    /// </summary>
    [Parameter]
    public BubbleAlign Align { get; set; } = BubbleAlign.Start;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// A <see cref="BubbleContent"/> and optional <see cref="BubbleReactions"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the bubble's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Each variant styles the BubbleContent child; the hover rule applies when that content
    // is a button or link. The default uses the text colour rather than the flame, which the
    // design system reserves for the primary action.
    private static string VariantClasses(BubbleVariant variant) => variant switch
    {
        BubbleVariant.Default =>
            "*:data-[slot=bubble-content]:bg-fg *:data-[slot=bubble-content]:text-surface " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-fg/85",
        BubbleVariant.Secondary =>
            "*:data-[slot=bubble-content]:bg-surface-raised *:data-[slot=bubble-content]:text-fg " +
            "*:data-[slot=bubble-content]:shadow-hairline " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-secondary-hover",
        BubbleVariant.Muted =>
            "*:data-[slot=bubble-content]:bg-surface-sunken *:data-[slot=bubble-content]:text-fg " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-hover",
        BubbleVariant.Tinted =>
            "*:data-[slot=bubble-content]:bg-selection *:data-[slot=bubble-content]:text-fg " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-hover",
        BubbleVariant.Outline =>
            "*:data-[slot=bubble-content]:bg-transparent *:data-[slot=bubble-content]:text-fg " +
            "*:data-[slot=bubble-content]:shadow-hairline-strong " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-hover",
        BubbleVariant.Ghost =>
            "*:data-[slot=bubble-content]:rounded-none *:data-[slot=bubble-content]:bg-transparent " +
            "*:data-[slot=bubble-content]:p-0 *:data-[slot=bubble-content]:text-fg " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-hover",
        BubbleVariant.Destructive =>
            "*:data-[slot=bubble-content]:bg-danger/10 *:data-[slot=bubble-content]:text-danger " +
            "[&>[data-slot=bubble-content]:is(button,a):hover]:bg-danger/15",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string VariantName(BubbleVariant variant) => variant switch
    {
        BubbleVariant.Default => "default",
        BubbleVariant.Secondary => "secondary",
        BubbleVariant.Muted => "muted",
        BubbleVariant.Tinted => "tinted",
        BubbleVariant.Outline => "outline",
        BubbleVariant.Ghost => "ghost",
        BubbleVariant.Destructive => "destructive",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
