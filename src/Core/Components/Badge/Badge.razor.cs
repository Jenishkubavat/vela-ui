// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A small pill for a status or label, such as "Approved" or "Beta".
/// </summary>
/// <remarks>
/// <para>
/// The word carries the meaning and colour is the second signal, so a status still reads
/// in greyscale. For a status, put a dot before the word and mark it as an icon:
/// <c>&lt;Badge Variant="BadgeVariant.Outline"&gt;&lt;span data-icon="inline-start" class="size-1.5 rounded-full bg-success"&gt;&lt;/span&gt;Approved&lt;/Badge&gt;</c>.
/// </para>
/// <para>
/// A badge isn't focusable. To make a link look like one, use <see cref="GetClasses"/>
/// on the <c>&lt;a&gt;</c> or <c>NavLink</c>.
/// </para>
/// </remarks>
public partial class Badge
{
    // Background and edge (box-shadow) are left to the variants, so each is set in one place.
    private const string LayoutClasses =
        "group/badge inline-flex h-[1.375rem] w-fit shrink-0 items-center justify-center gap-1.5 overflow-hidden " +
        "rounded-full px-2 text-caption whitespace-nowrap " +
        "has-data-[icon=inline-start]:ps-1.5 has-data-[icon=inline-end]:pe-1.5";

    private const string MotionClasses =
        "transition-colors duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    private const string FocusClasses =
        "outline-none focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-focus";

    private const string InvalidClasses = "aria-invalid:shadow-control-invalid";

    private const string IconClasses = "[&>svg]:pointer-events-none [&>svg]:size-3!";

    /// <summary>
    /// The visual style. Defaults to <see cref="BadgeVariant.Default"/>.
    /// </summary>
    [Parameter]
    public BadgeVariant Variant { get; set; } = BadgeVariant.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The badge's word, optionally with a dot or icon.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the badge's <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes a <see cref="Badge"/> with this variant uses, so a link can look
    /// like one: <c>&lt;a href="/changelog" class="@Badge.GetClasses(BadgeVariant.Outline)"&gt;</c>.
    /// </summary>
    public static string GetClasses(BadgeVariant variant = BadgeVariant.Default, string? additionalClasses = null) =>
        ClassNames.Join(
            LayoutClasses,
            MotionClasses,
            FocusClasses,
            InvalidClasses,
            IconClasses,
            VariantClasses(variant),
            additionalClasses);

    // [a]:hover only applies when the badge is a link, styled through GetClasses.
    private static string VariantClasses(BadgeVariant variant) => variant switch
    {
        BadgeVariant.Default =>
            "bg-transparent text-fg shadow-hairline-strong [a]:hover:bg-hover",
        BadgeVariant.Secondary =>
            "bg-surface-raised text-fg-muted shadow-hairline [a]:hover:bg-secondary-hover",
        BadgeVariant.Destructive =>
            "bg-transparent text-danger shadow-hairline-strong [a]:hover:bg-danger/10",
        BadgeVariant.Outline =>
            "bg-transparent text-fg-muted shadow-hairline-strong [a]:hover:bg-hover [a]:hover:text-fg",
        BadgeVariant.Ghost =>
            "bg-transparent text-fg-muted hover:bg-hover hover:text-fg",
        BadgeVariant.Link =>
            "bg-transparent text-fg underline decoration-border-control underline-offset-[0.2em] hover:decoration-fg",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string VariantName(BadgeVariant variant) => variant switch
    {
        BadgeVariant.Default => "default",
        BadgeVariant.Secondary => "secondary",
        BadgeVariant.Destructive => "destructive",
        BadgeVariant.Outline => "outline",
        BadgeVariant.Ghost => "ghost",
        BadgeVariant.Link => "link",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
