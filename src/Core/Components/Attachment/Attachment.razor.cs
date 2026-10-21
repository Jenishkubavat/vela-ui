// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A file attached to a message or form, with its upload state: a thumbnail, its name and
/// details, and actions such as remove.
/// </summary>
/// <remarks>
/// <para>
/// Compose it from <see cref="AttachmentMedia"/>, <see cref="AttachmentContent"/> (with
/// <see cref="AttachmentTitle"/> and <see cref="AttachmentDescription"/>),
/// <see cref="AttachmentActions"/>, and an optional <see cref="AttachmentTrigger"/> that
/// makes the whole card open the file. Line several up in an <see cref="AttachmentGroup"/>.
/// </para>
/// <para>
/// <see cref="State"/> only changes the look. Say the state in text too, in the description
/// ("Uploading, 40%" or "Upload failed"), and give the description <c>aria-live="polite"</c>
/// if the change should be announced. The component renders under static server rendering;
/// its buttons need an interactive render mode.
/// </para>
/// </remarks>
public partial class Attachment
{
    // group/attachment lets the parts follow data-state, data-size, and data-orientation.
    private const string LayoutClasses =
        "group/attachment relative flex w-fit max-w-full min-w-0 shrink-0 flex-wrap";

    // A card with a hairline edge. A file that can be opened lifts its edge on hover.
    private const string ColorClasses =
        "bg-surface text-fg shadow-card has-[>a,>button]:hover:shadow-card-hover";

    // Idle is an empty slot, so its edge is dashed: a real border, since rings can't dash.
    private const string StateClasses =
        "data-[state=error]:shadow-control-invalid " +
        "data-[state=idle]:border data-[state=idle]:border-dashed data-[state=idle]:border-border-strong " +
        "data-[state=idle]:shadow-none";

    private const string MotionClasses =
        "transition-shadow duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    // The trigger fills the card, so the card draws its focus ring.
    private const string FocusClasses =
        "has-[>[data-slot=attachment-trigger]:focus-visible]:outline-2 " +
        "has-[>[data-slot=attachment-trigger]:focus-visible]:outline-offset-2 " +
        "has-[>[data-slot=attachment-trigger]:focus-visible]:outline-focus";

    /// <summary>
    /// Where the file is in being added. Defaults to <see cref="AttachmentState.Done"/>.
    /// </summary>
    [Parameter]
    public AttachmentState State { get; set; } = AttachmentState.Done;

    /// <summary>
    /// How big the attachment is. Defaults to <see cref="AttachmentSize.Default"/>.
    /// </summary>
    [Parameter]
    public AttachmentSize Size { get; set; } = AttachmentSize.Default;

    /// <summary>
    /// A row or a tile. Defaults to <see cref="AttachmentOrientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public AttachmentOrientation Orientation { get; set; } = AttachmentOrientation.Horizontal;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The attachment's parts.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the attachment's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes an <see cref="Attachment"/> with these settings would use, so
    /// another element can look like one. Set <c>data-state</c>, <c>data-size</c>, and
    /// <c>data-orientation</c> on it too, since the parts follow them.
    /// </summary>
    public static string GetClasses(
        AttachmentSize size = AttachmentSize.Default,
        AttachmentOrientation orientation = AttachmentOrientation.Horizontal,
        string? additionalClasses = null) =>
        ClassNames.Join(
            LayoutClasses,
            ColorClasses,
            StateClasses,
            MotionClasses,
            FocusClasses,
            SizeClasses(size),
            OrientationClasses(orientation),
            additionalClasses);

    // Padding depends on what's inside: a thumbnail-only attachment is padded evenly.
    private static string SizeClasses(AttachmentSize size) => size switch
    {
        AttachmentSize.Default =>
            "gap-2 rounded-xl text-body has-data-[slot=attachment-content]:px-2.5 " +
            "has-data-[slot=attachment-content]:py-2 has-data-[slot=attachment-media]:p-2",
        AttachmentSize.Small =>
            "gap-2.5 rounded-xl text-caption has-data-[slot=attachment-content]:px-2 " +
            "has-data-[slot=attachment-content]:py-1.5 has-data-[slot=attachment-media]:p-1.5",
        AttachmentSize.ExtraSmall =>
            "gap-1.5 rounded-lg text-caption has-data-[slot=attachment-content]:px-1.5 " +
            "has-data-[slot=attachment-content]:py-1 has-data-[slot=attachment-media]:p-1",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    private static string OrientationClasses(AttachmentOrientation orientation) => orientation switch
    {
        AttachmentOrientation.Horizontal => "min-w-40 items-center",
        AttachmentOrientation.Vertical => "w-24 flex-col has-data-[slot=attachment-content]:w-30",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };

    private static string StateName(AttachmentState state) => state switch
    {
        AttachmentState.Idle => "idle",
        AttachmentState.Uploading => "uploading",
        AttachmentState.Processing => "processing",
        AttachmentState.Error => "error",
        AttachmentState.Done => "done",
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
    };

    private static string SizeName(AttachmentSize size) => size switch
    {
        AttachmentSize.Default => "default",
        AttachmentSize.Small => "sm",
        AttachmentSize.ExtraSmall => "xs",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    private static string OrientationName(AttachmentOrientation orientation) => orientation switch
    {
        AttachmentOrientation.Horizontal => "horizontal",
        AttachmentOrientation.Vertical => "vertical",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };
}
