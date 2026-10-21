// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// Content beside the control in an <see cref="InputGroup"/>: an icon, an
/// <see cref="InputGroupText"/>, a <see cref="Kbd"/>, or an <see cref="InputGroupButton"/>.
/// </summary>
/// <remarks>
/// Icons here are decorative, so mark them <c>aria-hidden="true"</c>. Text that changes the
/// meaning of the value, such as a currency or a domain, should also be in the control's
/// label or <c>aria-describedby</c>.
/// </remarks>
public partial class InputGroupAddon
{
    private const string Classes =
        "flex h-auto items-center justify-center gap-2 py-1.5 text-label text-fg-muted select-none " +
        "group-has-disabled/input-group:text-fg-faint [&>kbd]:rounded-sm [&>svg:not([class*='size-'])]:size-4";

    /// <summary>
    /// Where the addon sits. Defaults to <see cref="InputGroupAddonAlign.InlineStart"/>.
    /// </summary>
    [Parameter]
    public InputGroupAddonAlign Align { get; set; } = InputGroupAddonAlign.InlineStart;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The addon's content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the addon's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // A button or shortcut at the edge pulls outwards a little, so its own padding lines up
    // with the group's rounded corner.
    private static string AlignClasses(InputGroupAddonAlign align) => align switch
    {
        InputGroupAddonAlign.InlineStart =>
            "order-first ps-2 has-[>button]:-ms-[0.3rem] has-[>kbd]:-ms-[0.15rem]",
        InputGroupAddonAlign.InlineEnd =>
            "order-last pe-2 has-[>button]:-me-[0.3rem] has-[>kbd]:-me-[0.15rem]",
        InputGroupAddonAlign.BlockStart =>
            "order-first w-full justify-start px-2.5 pt-2 group-has-[>input]/input-group:pt-2 [.border-b]:pb-2",
        InputGroupAddonAlign.BlockEnd =>
            "order-last w-full justify-start px-2.5 pb-2 group-has-[>input]/input-group:pb-2 [.border-t]:pt-2",
        _ => throw new ArgumentOutOfRangeException(nameof(align), align, null),
    };

    private static string AlignName(InputGroupAddonAlign align) => align switch
    {
        InputGroupAddonAlign.InlineStart => "inline-start",
        InputGroupAddonAlign.InlineEnd => "inline-end",
        InputGroupAddonAlign.BlockStart => "block-start",
        InputGroupAddonAlign.BlockEnd => "block-end",
        _ => throw new ArgumentOutOfRangeException(nameof(align), align, null),
    };
}
