// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// An <see cref="Attachment"/>'s thumbnail: a file-type icon, a <see cref="Spinner"/> while
/// it uploads, or a preview image. It follows the attachment's size and state.
/// </summary>
/// <remarks>
/// The thumbnail is decorative next to the file name: mark icons <c>aria-hidden="true"</c>
/// and give preview images an empty <c>alt</c>.
/// </remarks>
public partial class AttachmentMedia
{
    private const string LayoutClasses =
        "relative flex aspect-square w-10 shrink-0 items-center justify-center overflow-hidden rounded-lg " +
        "group-data-[size=sm]/attachment:w-8 group-data-[size=xs]/attachment:w-7 " +
        "group-data-[size=xs]/attachment:rounded-md group-data-[orientation=vertical]/attachment:w-full";

    private const string ColorClasses =
        "bg-surface-sunken text-fg " +
        "group-data-[state=error]/attachment:bg-danger/10 group-data-[state=error]/attachment:text-danger";

    // Icons and spinners grow with a vertical tile and shrink with the smallest size.
    private const string IconClasses =
        "[&_svg]:pointer-events-none [&_svg:not([class*='size-'])]:size-4 " +
        "group-data-[orientation=vertical]/attachment:[&_svg:not([class*='size-'])]:size-6 " +
        "group-data-[size=xs]/attachment:[&_svg:not([class*='size-'])]:size-3.5 " +
        "group-data-[orientation=vertical]/attachment:*:data-[slot=spinner]:size-6!";

    /// <summary>
    /// What the thumbnail shows. Defaults to <see cref="AttachmentMediaVariant.Icon"/>.
    /// </summary>
    [Parameter]
    public AttachmentMediaVariant Variant { get; set; } = AttachmentMediaVariant.Icon;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The icon, spinner, or <c>&lt;img&gt;</c>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the thumbnail's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // A preview stays dimmed until the file is ready, or while the slot is still empty.
    private static string VariantClasses(AttachmentMediaVariant variant) => variant switch
    {
        AttachmentMediaVariant.Icon => "",
        AttachmentMediaVariant.Image =>
            "opacity-60 group-data-[state=done]/attachment:opacity-100 group-data-[state=idle]/attachment:opacity-100 " +
            "[&>img]:aspect-square [&>img]:w-full [&>img]:object-cover",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    private static string VariantName(AttachmentMediaVariant variant) => variant switch
    {
        AttachmentMediaVariant.Icon => "icon",
        AttachmentMediaVariant.Image => "image",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
