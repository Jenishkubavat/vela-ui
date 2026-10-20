// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Vela.UI.Components;

/// <summary>
/// A native <c>&lt;button&gt;</c> styled with the Vela design system.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="ButtonVariant.Default"/> is the primary action and the only variant that uses
/// the flame, so a view should have one. Keyboard activation (Enter and Space), focus, and
/// the disabled state come from the browser. The button renders correctly under static
/// server rendering; only <see cref="OnClick"/> needs an interactive render mode.
/// </para>
/// <para>
/// Show a shortcut with a <see cref="Kbd"/> inside the button, and a loading state with a
/// <see cref="Spinner"/> plus <see cref="Disabled"/>, keeping the label ("Saving"). Mark an
/// icon with <c>data-icon="inline-start"</c> or <c>data-icon="inline-end"</c> to tighten the
/// padding on that side. To make a link or <c>NavLink</c> look like a button, use
/// <see cref="GetClasses"/>.
/// </para>
/// </remarks>
public partial class Button
{
    // The groups below apply to every button. Rounding, icon size, text size, background,
    // and edge (box-shadow) are left to the variant and size groups further down:
    // ClassNames.Join doesn't resolve conflicts, so each may be set in exactly one place.

    // group/button lets content react to the button's state, e.g. group-hover/button:.
    private const string LayoutClasses =
        "group/button inline-flex shrink-0 cursor-pointer items-center justify-center whitespace-nowrap select-none " +
        "font-semibold leading-none no-underline";

    // Hover and press are written not-disabled: so a disabled button never lights up. Unlike
    // enabled:, that also works when GetClasses styles a link.
    private const string MotionClasses =
        "transition-[background-color,box-shadow,transform,color] duration-(--vela-quick) ease-vela " +
        "motion-reduce:transition-none not-disabled:active:scale-97";

    private const string FocusClasses =
        "outline-none focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-focus";

    private const string InvalidClasses =
        "not-disabled:aria-invalid:shadow-control-invalid";

    private const string DisabledClasses =
        "disabled:cursor-not-allowed disabled:bg-transparent disabled:text-fg-faint disabled:shadow-hairline";

    private const string IconClasses =
        "[&_svg]:pointer-events-none [&_svg]:shrink-0";

    private const string IconSize3 = "[&_svg:not([class*='size-'])]:size-3";
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
        ClassNames.Join(
            LayoutClasses,
            MotionClasses,
            FocusClasses,
            InvalidClasses,
            DisabledClasses,
            IconClasses,
            VariantClasses(variant),
            SizeClasses(size),
            additionalClasses);

    private static string VariantClasses(ButtonVariant variant) => variant switch
    {
        ButtonVariant.Default =>
            "bg-flame text-on-flame shadow-button-primary not-disabled:hover:bg-flame-hover",
        ButtonVariant.Destructive =>
            "bg-transparent text-danger shadow-hairline-strong " +
            "not-disabled:hover:bg-danger not-disabled:hover:text-on-danger not-disabled:hover:shadow-none",
        ButtonVariant.Outline =>
            "bg-transparent text-fg shadow-hairline-strong not-disabled:hover:bg-hover aria-expanded:bg-hover",
        ButtonVariant.Secondary =>
            "bg-surface-raised text-fg shadow-button-secondary " +
            "not-disabled:hover:bg-secondary-hover aria-expanded:bg-secondary-hover",
        ButtonVariant.Ghost =>
            "bg-transparent text-fg-muted not-disabled:hover:bg-hover not-disabled:hover:text-fg " +
            "aria-expanded:bg-hover aria-expanded:text-fg",
        ButtonVariant.Link =>
            "bg-transparent text-fg underline decoration-border-control decoration-1 underline-offset-[0.2em] " +
            "not-disabled:hover:decoration-fg",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Padding uses logical properties (ps/pe) so icon spacing flips in right-to-left layouts.
    private static string SizeClasses(ButtonSize size) => size switch
    {
        ButtonSize.Default =>
            $"h-8 gap-2 rounded-md px-3.5 text-label has-data-[icon=inline-start]:ps-3 has-data-[icon=inline-end]:pe-3 {IconSize4}",
        ButtonSize.ExtraSmall =>
            $"h-6 gap-1 rounded-sm px-2 text-xs has-data-[icon=inline-start]:ps-1.5 has-data-[icon=inline-end]:pe-1.5 {IconSize3}",
        ButtonSize.Small =>
            $"h-7 gap-1.5 rounded-md px-2.5 text-xs has-data-[icon=inline-start]:ps-2 has-data-[icon=inline-end]:pe-2 {IconSize4}",
        ButtonSize.Large =>
            $"h-10 gap-2 rounded-md px-4.5 text-sm has-data-[icon=inline-start]:ps-3.5 has-data-[icon=inline-end]:pe-3.5 {IconSize4}",
        ButtonSize.Icon =>
            $"size-8 rounded-md {IconSize4}",
        ButtonSize.IconExtraSmall =>
            $"size-6 rounded-sm {IconSize3}",
        ButtonSize.IconSmall =>
            $"size-7 rounded-md {IconSize4}",
        ButtonSize.IconLarge =>
            $"size-10 rounded-md {IconSize4}",
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
