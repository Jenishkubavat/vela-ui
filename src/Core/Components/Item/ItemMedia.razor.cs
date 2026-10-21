// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// The icon, image, or avatar at the start of an <see cref="Item"/>. When the item has a
/// description, the media lines up with the title instead of the middle.
/// </summary>
/// <remarks>
/// Media is usually decorative: mark icons <c>aria-hidden="true"</c> and give images an
/// empty <c>alt</c> when the title already says what they show.
/// </remarks>
public partial class ItemMedia
{
    private const string Classes =
        "flex shrink-0 items-center justify-center gap-2 [&_svg]:pointer-events-none " +
        "group-has-data-[slot=item-description]/item:translate-y-0.5 " +
        "group-has-data-[slot=item-description]/item:self-start";

    /// <summary>
    /// What the media holds. Defaults to <see cref="ItemMediaVariant.Default"/>.
    /// </summary>
    [Parameter]
    public ItemMediaVariant Variant { get; set; } = ItemMediaVariant.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The icon, image, or avatar.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the media's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private static string VariantClasses(ItemMediaVariant variant) => variant switch
    {
        ItemMediaVariant.Default => "bg-transparent",
        ItemMediaVariant.Icon => "[&_svg:not([class*='size-'])]:size-4",
        ItemMediaVariant.Image =>
            "size-10 overflow-hidden rounded-sm group-data-[size=sm]/item:size-8 group-data-[size=xs]/item:size-6 " +
            "[&_img]:size-full [&_img]:object-cover",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    private static string VariantName(ItemMediaVariant variant) => variant switch
    {
        ItemMediaVariant.Default => "default",
        ItemMediaVariant.Icon => "icon",
        ItemMediaVariant.Image => "image",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
