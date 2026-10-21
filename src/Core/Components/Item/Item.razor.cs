// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A row of content: media, a title and description, and actions. Use it for settings rows,
/// people, files, or search results.
/// </summary>
/// <remarks>
/// <para>
/// Compose it from <see cref="ItemMedia"/>, <see cref="ItemContent"/> (with
/// <see cref="ItemTitle"/> and <see cref="ItemDescription"/>), <see cref="ItemActions"/>,
/// <see cref="ItemHeader"/>, and <see cref="ItemFooter"/>. List several in an
/// <see cref="ItemGroup"/>, with <c>role="listitem"</c> on each.
/// </para>
/// <para>
/// To make the whole row a link, put the parts in an <c>&lt;a&gt;</c> or <c>NavLink</c> with
/// <c>class="@Item.GetClasses()"</c>, the Blazor counterpart of shadcn/ui's <c>asChild</c>.
/// A link item highlights on hover and shows the focus ring.
/// </para>
/// </remarks>
public partial class Item
{
    // group/item lets the parts follow data-size and the presence of a description.
    private const string LayoutClasses =
        "group/item flex w-full flex-wrap items-center rounded-lg text-body text-fg";

    private const string MotionClasses =
        "transition-colors duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    // Only a link item can be focused or hovered.
    private const string InteractiveClasses =
        "outline-none focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-focus " +
        "[a&]:hover:bg-hover";

    /// <summary>
    /// The look. Defaults to <see cref="ItemVariant.Default"/>.
    /// </summary>
    [Parameter]
    public ItemVariant Variant { get; set; } = ItemVariant.Default;

    /// <summary>
    /// The padding and spacing. Defaults to <see cref="ItemSize.Default"/>.
    /// </summary>
    [Parameter]
    public ItemSize Size { get; set; } = ItemSize.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The item's parts.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>role="listitem"</c>, applied to the item's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes an <see cref="Item"/> with these settings would use, so a link can
    /// look like one: <c>&lt;a href="/settings" class="@Item.GetClasses(ItemVariant.Outline)"&gt;</c>.
    /// Add <c>data-slot="item"</c> and <c>data-size</c> to the link if the parts should follow
    /// a size other than the default.
    /// </summary>
    public static string GetClasses(
        ItemVariant variant = ItemVariant.Default,
        ItemSize size = ItemSize.Default,
        string? additionalClasses = null) =>
        ClassNames.Join(
            LayoutClasses,
            MotionClasses,
            InteractiveClasses,
            VariantClasses(variant),
            SizeClasses(size),
            additionalClasses);

    private static string VariantClasses(ItemVariant variant) => variant switch
    {
        ItemVariant.Default => "bg-transparent",
        ItemVariant.Outline => "bg-transparent shadow-hairline",
        ItemVariant.Muted => "bg-hover",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    private static string SizeClasses(ItemSize size) => size switch
    {
        ItemSize.Default => "gap-2.5 px-3 py-2.5",
        ItemSize.Small => "gap-2.5 px-3 py-2.5",
        ItemSize.ExtraSmall => "gap-2 px-2.5 py-2 in-data-[slot=dropdown-menu-content]:p-0",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    private static string VariantName(ItemVariant variant) => variant switch
    {
        ItemVariant.Default => "default",
        ItemVariant.Outline => "outline",
        ItemVariant.Muted => "muted",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    private static string SizeName(ItemSize size) => size switch
    {
        ItemSize.Default => "default",
        ItemSize.Small => "sm",
        ItemSize.ExtraSmall => "xs",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };
}
