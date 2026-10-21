// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// A person, by photo or initials: round, graphite, with a fine outline. Built on
/// <see cref="AvatarPrimitive"/>. Put an <see cref="AvatarImage"/> and an
/// <see cref="AvatarFallback"/> inside it.
/// </summary>
/// <remarks>
/// <para>
/// The initials show while the photo loads, if it fails, and under static server
/// rendering until the photo paints over them. Removing a broken photo needs an interactive
/// render mode.
/// </para>
/// <para>
/// When the person's name is shown next to the avatar, add <c>aria-hidden="true"</c> so
/// screen readers don't read the initials too. When it isn't, add <c>role="img"</c> and
/// <c>aria-label</c> with the name.
/// </para>
/// </remarks>
public partial class Avatar
{
    // group/avatar lets the parts follow data-size and data-current.
    private const string LayoutClasses =
        "group/avatar relative flex shrink-0 select-none rounded-full";

    // The outline sits on a pseudo-element above the photo, so it shows on photos too.
    // The current person gets a ring in the text colour rather than the flame.
    private const string EdgeClasses =
        "after:pointer-events-none after:absolute after:inset-0 after:rounded-full after:shadow-hairline-strong " +
        "data-current:after:shadow-[inset_0_0_0_1.5px_var(--vela-text)]";

    /// <summary>
    /// How big the avatar is. Defaults to <see cref="AvatarSize.Default"/> (22px).
    /// </summary>
    [Parameter]
    public AvatarSize Size { get; set; } = AvatarSize.Default;

    /// <summary>
    /// Marks the avatar as the signed-in person's, so "mine" is easy to spot in a list of
    /// owners. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool Current { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The image, fallback, and an optional <see cref="AvatarBadge"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-hidden</c> or <c>role</c> and <c>aria-label</c>,
    /// applied to the avatar's <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private static string SizeClasses(AvatarSize size) => size switch
    {
        AvatarSize.Default => "size-5.5",
        AvatarSize.Small => "size-4.5",
        AvatarSize.Large => "size-8",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    private static string SizeName(AvatarSize size) => size switch
    {
        AvatarSize.Default => "default",
        AvatarSize.Small => "sm",
        AvatarSize.Large => "lg",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };
}
