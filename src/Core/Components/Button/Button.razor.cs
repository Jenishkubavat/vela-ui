// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Vela.UI.Components;

/// <summary>
/// A native <c>&lt;button&gt;</c> styled with the Vela UI theme tokens.
/// </summary>
/// <remarks>
/// <para>
/// Keyboard activation (Enter and Space), focus, and the disabled state come from the
/// browser. The button renders correctly under static server rendering; only
/// <see cref="OnClick"/> needs an interactive render mode.
/// </para>
/// <para>
/// Mark an icon with <c>data-icon="inline-start"</c> or <c>data-icon="inline-end"</c> to
/// tighten the padding on that side. Setting <c>aria-invalid</c> or <c>aria-expanded</c>
/// through <see cref="AdditionalAttributes"/> applies the matching styles. To make a link
/// or <c>NavLink</c> look like a button, use <see cref="GetClasses"/>.
/// </para>
/// </remarks>
public partial class Button
{
    // Rounding, icon size, focus colors, and border colors are deliberately left to the
    // variant and size groups. ClassNames.Join doesn't resolve conflicts, so each of
    // those may be set in exactly one group.
    private const string BaseClasses =
        "group/button inline-flex shrink-0 items-center justify-center whitespace-nowrap " +
        "border bg-clip-padding text-sm font-medium select-none outline-none " +
        "transition-all motion-reduce:transition-none " +
        "focus-visible:ring-3 active:not-aria-[haspopup]:translate-y-px " +
        "aria-invalid:border-destructive aria-invalid:ring-3 aria-invalid:ring-destructive/20 " +
        "dark:aria-invalid:border-destructive/50 dark:aria-invalid:ring-destructive/40 " +
        "disabled:pointer-events-none disabled:opacity-50 " +
        "[&_svg]:pointer-events-none [&_svg]:shrink-0";

    private const string StandardFocus = "focus-visible:border-ring focus-visible:ring-ring/50";

    private const string IconSize3 = "[&_svg:not([class*='size-'])]:size-3";
    private const string IconSize3_5 = "[&_svg:not([class*='size-'])]:size-3.5";
    private const string IconSize4 = "[&_svg:not([class*='size-'])]:size-4";

    /// <summary>
    /// The visual style. Defaults to <see cref="ButtonVariant.Default"/>.
    /// </summary>
    [Parameter]
    public ButtonVariant Variant { get; set; } = ButtonVariant.Default;

    /// <summary>
    /// The height and padding. Defaults to <see cref="ButtonSize.Default"/>.
    /// </summary>
    [Parameter]
    public ButtonSize Size { get; set; } = ButtonSize.Default;

    /// <summary>
    /// Prevents the button from being focused or activated.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The HTML <c>type</c> attribute: <c>button</c>, <c>submit</c>, or <c>reset</c>.
    /// Defaults to <c>button</c> rather than the HTML default of <c>submit</c>, so a
    /// button inside a form doesn't submit it unless you ask it to.
    /// </summary>
    [Parameter]
    public string Type { get; set; } = "button";

    /// <summary>
    /// Extra CSS classes, added after the variant and size classes. Classes that don't
    /// clash with the defaults, such as <c>w-full</c>, just work. To replace a default,
    /// add Tailwind's important modifier: <c>Class="rounded-full!"</c> makes a round button.
    /// See <see cref="ClassNames.Join"/> for why.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Invoked when the button is clicked or activated from the keyboard.
    /// Requires an interactive render mode.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// The button's content, usually text and an optional icon.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-label</c>, <c>id</c>, or <c>form</c>, applied to
    /// the <c>&lt;button&gt;</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes a <see cref="Button"/> with these settings would use, so another
    /// element can look like a button while keeping its own behavior:
    /// <c>&lt;NavLink href="/login" class="@Button.GetClasses(ButtonVariant.Outline)"&gt;</c>.
    /// </summary>
    /// <remarks>
    /// This is the Blazor counterpart of shadcn/ui's <c>asChild</c> and <c>buttonVariants</c>.
    /// The element keeps its own semantics, so a link stays a link for keyboard and
    /// screen reader users.
    /// </remarks>
    public static string GetClasses(
        ButtonVariant variant = ButtonVariant.Default,
        ButtonSize size = ButtonSize.Default,
        string? additionalClasses = null) =>
        ClassNames.Join(BaseClasses, VariantClasses(variant), SizeClasses(size), additionalClasses);

    private static string VariantClasses(ButtonVariant variant) => variant switch
    {
        ButtonVariant.Default =>
            $"border-transparent bg-primary text-primary-foreground hover:bg-primary/80 {StandardFocus}",
        ButtonVariant.Destructive =>
            "border-transparent bg-destructive/10 text-destructive hover:bg-destructive/20 " +
            "focus-visible:border-destructive/40 focus-visible:ring-destructive/20 " +
            "dark:bg-destructive/20 dark:hover:bg-destructive/30 dark:focus-visible:ring-destructive/40",
        ButtonVariant.Outline =>
            "border-border bg-background hover:bg-muted hover:text-foreground " +
            "aria-expanded:bg-muted aria-expanded:text-foreground " +
            $"dark:border-input dark:bg-input/30 dark:hover:bg-input/50 {StandardFocus}",
        ButtonVariant.Secondary =>
            "border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80 " +
            $"aria-expanded:bg-secondary aria-expanded:text-secondary-foreground {StandardFocus}",
        ButtonVariant.Ghost =>
            "border-transparent hover:bg-muted hover:text-foreground dark:hover:bg-muted/50 " +
            $"aria-expanded:bg-muted aria-expanded:text-foreground {StandardFocus}",
        ButtonVariant.Link =>
            $"border-transparent text-primary underline-offset-4 hover:underline {StandardFocus}",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Padding uses logical properties (ps/pe) so icon spacing flips in right-to-left layouts.
    private static string SizeClasses(ButtonSize size) => size switch
    {
        ButtonSize.Default =>
            $"h-8 gap-1.5 rounded-lg px-2.5 has-data-[icon=inline-start]:ps-2 has-data-[icon=inline-end]:pe-2 {IconSize4}",
        ButtonSize.ExtraSmall =>
            "h-6 gap-1 rounded-[min(var(--radius-md),10px)] px-2 text-xs " +
            "has-data-[icon=inline-start]:ps-1.5 has-data-[icon=inline-end]:pe-1.5 " +
            $"in-data-[slot=button-group]:rounded-lg {IconSize3}",
        ButtonSize.Small =>
            "h-7 gap-1 rounded-[min(var(--radius-md),12px)] px-2.5 text-[0.8rem] " +
            "has-data-[icon=inline-start]:ps-1.5 has-data-[icon=inline-end]:pe-1.5 " +
            $"in-data-[slot=button-group]:rounded-lg {IconSize3_5}",
        ButtonSize.Large =>
            $"h-9 gap-1.5 rounded-lg px-2.5 has-data-[icon=inline-start]:ps-2 has-data-[icon=inline-end]:pe-2 {IconSize4}",
        ButtonSize.Icon =>
            $"size-8 rounded-lg {IconSize4}",
        ButtonSize.IconExtraSmall =>
            $"size-6 rounded-[min(var(--radius-md),10px)] in-data-[slot=button-group]:rounded-lg {IconSize3}",
        ButtonSize.IconSmall =>
            $"size-7 rounded-[min(var(--radius-md),12px)] in-data-[slot=button-group]:rounded-lg {IconSize4}",
        ButtonSize.IconLarge =>
            $"size-9 rounded-lg {IconSize4}",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    // data-variant and data-size use shadcn/ui's names, so selectors written for shadcn
    // (for example in-data-[size=sm]) work unchanged.
    private static string VariantName(ButtonVariant variant) => variant switch
    {
        ButtonVariant.Default => "default",
        ButtonVariant.Destructive => "destructive",
        ButtonVariant.Outline => "outline",
        ButtonVariant.Secondary => "secondary",
        ButtonVariant.Ghost => "ghost",
        ButtonVariant.Link => "link",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    private static string SizeName(ButtonSize size) => size switch
    {
        ButtonSize.Default => "default",
        ButtonSize.ExtraSmall => "xs",
        ButtonSize.Small => "sm",
        ButtonSize.Large => "lg",
        ButtonSize.Icon => "icon",
        ButtonSize.IconExtraSmall => "icon-xs",
        ButtonSize.IconSmall => "icon-sm",
        ButtonSize.IconLarge => "icon-lg",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };
}
