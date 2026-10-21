// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// A two-state button, such as Bold in a text toolbar, styled on top of
/// <see cref="TogglePrimitive"/>. Bind its state with <c>@bind-Pressed</c>.
/// </summary>
/// <remarks>
/// Pressed shows as a raised graphite key in the text colour; like other on states it
/// never uses the flame. An icon-only toggle needs an <c>aria-label</c>. Toggling needs an
/// interactive render mode.
/// </remarks>
public partial class Toggle
{
    // Background and edge are left to the variant groups, so each is set in one place.
    private const string LayoutClasses =
        "group/toggle inline-flex shrink-0 cursor-pointer items-center justify-center gap-1.5 rounded-md " +
        "font-semibold leading-none whitespace-nowrap select-none [&_svg]:pointer-events-none [&_svg]:shrink-0";

    private const string ColorClasses =
        "text-fg-muted not-disabled:hover:bg-hover not-disabled:hover:text-fg " +
        "data-[state=on]:bg-surface-raised data-[state=on]:text-fg";

    private const string MotionClasses =
        "transition-[background-color,box-shadow,color] duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    private const string FocusClasses =
        "outline-none focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-focus";

    private const string DisabledClasses = "disabled:cursor-not-allowed disabled:text-fg-faint";

    /// <summary>
    /// Whether the toggle is on. Use <c>@bind-Pressed</c> to keep it in sync.
    /// </summary>
    [Parameter]
    public bool Pressed { get; set; }

    /// <summary>
    /// Invoked with the new state when the person toggles it.
    /// </summary>
    [Parameter]
    public EventCallback<bool> PressedChanged { get; set; }

    /// <summary>
    /// The visual style. Defaults to <see cref="ToggleVariant.Default"/>.
    /// </summary>
    [Parameter]
    public ToggleVariant Variant { get; set; } = ToggleVariant.Default;

    /// <summary>
    /// The height. Defaults to <see cref="ToggleSize.Default"/>.
    /// </summary>
    [Parameter]
    public ToggleSize Size { get; set; } = ToggleSize.Default;

    /// <summary>
    /// Prevents the toggle from being focused or pressed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The label: short text, or an icon with an <c>aria-label</c> on the toggle.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-label</c>, applied to the <c>&lt;button&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns the classes a <see cref="Toggle"/> with these settings uses, for an element
    /// that should look like one.
    /// </summary>
    public static string GetClasses(
        ToggleVariant variant = ToggleVariant.Default,
        ToggleSize size = ToggleSize.Default,
        string? additionalClasses = null) =>
        ClassNames.Join(
            LayoutClasses,
            ColorClasses,
            MotionClasses,
            FocusClasses,
            DisabledClasses,
            VariantClasses(variant),
            SizeClasses(size),
            additionalClasses);

    private static string VariantClasses(ToggleVariant variant) => variant switch
    {
        ToggleVariant.Default => "bg-transparent data-[state=on]:shadow-hairline-strong",
        ToggleVariant.Outline => "bg-transparent shadow-hairline-strong",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };

    // Padding uses logical properties (ps/pe) so icon spacing flips in right-to-left layouts.
    private static string SizeClasses(ToggleSize size) => size switch
    {
        ToggleSize.Default =>
            "h-8 min-w-8 px-2.5 text-label has-data-[icon=inline-start]:ps-2 has-data-[icon=inline-end]:pe-2 " +
            "[&_svg:not([class*='size-'])]:size-4",
        ToggleSize.Small =>
            "h-7 min-w-7 px-2 text-xs has-data-[icon=inline-start]:ps-1.5 has-data-[icon=inline-end]:pe-1.5 " +
            "[&_svg:not([class*='size-'])]:size-3.5",
        ToggleSize.Large =>
            "h-10 min-w-10 px-3 text-sm has-data-[icon=inline-start]:ps-2.5 has-data-[icon=inline-end]:pe-2.5 " +
            "[&_svg:not([class*='size-'])]:size-4",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    // Keeps this component's own state in step, so it doesn't reset the primitive when its
    // parent re-renders a toggle that isn't bound.
    private async Task OnPressedChanged(bool pressed)
    {
        Pressed = pressed;
        await PressedChanged.InvokeAsync(pressed);
    }
}
