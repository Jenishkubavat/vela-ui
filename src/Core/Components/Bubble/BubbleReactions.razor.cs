// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// Emoji reactions pinned to a corner of a <see cref="Bubble"/>.
/// </summary>
/// <remarks>
/// A ring in the surface colour cuts the reactions out of the bubble edge they overlap.
/// If people can react, make each reaction a small Button with an <c>aria-label</c> such
/// as "Thumbs up, 3 reactions", because an emoji alone is read inconsistently.
/// </remarks>
public partial class BubbleReactions
{
    private const string LayoutClasses =
        "absolute z-10 flex w-fit shrink-0 items-center justify-center gap-1 rounded-full bg-surface-raised " +
        "px-1.5 py-0.5 text-body-sm ring-3 ring-surface has-[button]:p-0";

    /// <summary>
    /// Which edge the reactions overlap. Defaults to <see cref="BubbleReactionsSide.Bottom"/>.
    /// </summary>
    [Parameter]
    public BubbleReactionsSide Side { get; set; } = BubbleReactionsSide.Bottom;

    /// <summary>
    /// Which corner along that edge. Defaults to <see cref="BubbleAlign.End"/>.
    /// </summary>
    [Parameter]
    public BubbleAlign Align { get; set; } = BubbleAlign.End;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The reactions, as text or small Buttons.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the reactions' <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private string SideClasses => Side switch
    {
        BubbleReactionsSide.Top => "top-0 -translate-y-3/4",
        BubbleReactionsSide.Bottom => "bottom-0 translate-y-3/4",
        _ => throw new ArgumentOutOfRangeException(nameof(Side), Side, null),
    };

    // Logical start/end so the corner flips in right-to-left layouts.
    private string AlignClasses => Align switch
    {
        BubbleAlign.Start => "start-3",
        BubbleAlign.End => "end-3",
        _ => throw new ArgumentOutOfRangeException(nameof(Align), Align, null),
    };
}
