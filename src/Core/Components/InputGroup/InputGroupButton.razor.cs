// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Vela.UI.Components;

/// <summary>
/// A small <see cref="Button"/> sized to sit inside an <see cref="InputGroupAddon"/>, such as
/// a copy, clear, or send button. It's a ghost button by default.
/// </summary>
/// <remarks>
/// Give icon-only buttons an <c>aria-label</c>. <see cref="OnClick"/> needs an interactive
/// render mode; to submit the surrounding form instead, set <see cref="Type"/> to
/// <c>submit</c>.
/// </remarks>
public partial class InputGroupButton
{
    /// <summary>
    /// The visual style. Defaults to <see cref="ButtonVariant.Ghost"/>.
    /// </summary>
    [Parameter]
    public ButtonVariant Variant { get; set; } = ButtonVariant.Ghost;

    /// <summary>
    /// The button's size. Defaults to <see cref="InputGroupButtonSize.ExtraSmall"/>.
    /// </summary>
    [Parameter]
    public InputGroupButtonSize Size { get; set; } = InputGroupButtonSize.ExtraSmall;

    /// <summary>
    /// The HTML <c>type</c> attribute. Defaults to <c>button</c>.
    /// </summary>
    [Parameter]
    public string Type { get; set; } = "button";

    /// <summary>
    /// Prevents the button from being focused or activated.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Invoked when the button is clicked or activated from the keyboard.
    /// Requires an interactive render mode.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the button classes.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The button's content, usually an icon or a short label.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>aria-label</c>, applied to the <c>&lt;button&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private static ButtonSize ButtonSizeFor(InputGroupButtonSize size) => size switch
    {
        InputGroupButtonSize.ExtraSmall => ButtonSize.ExtraSmall,
        InputGroupButtonSize.Small => ButtonSize.Small,
        InputGroupButtonSize.IconExtraSmall => ButtonSize.IconExtraSmall,
        InputGroupButtonSize.IconSmall => ButtonSize.IconSmall,
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };

    private static string SizeName(InputGroupButtonSize size) => size switch
    {
        InputGroupButtonSize.ExtraSmall => "xs",
        InputGroupButtonSize.Small => "sm",
        InputGroupButtonSize.IconExtraSmall => "icon-xs",
        InputGroupButtonSize.IconSmall => "icon-sm",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };
}
