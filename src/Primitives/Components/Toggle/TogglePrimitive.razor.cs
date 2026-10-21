// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled two-state button, such as Bold in a text toolbar. Bind its state with
/// <c>@bind-Pressed</c>; without a binding it keeps its own.
/// </summary>
/// <remarks>
/// It's a native <c>&lt;button&gt;</c> with <c>aria-pressed</c>, so Enter and Space toggle it
/// and screen readers announce "pressed" or "not pressed". Style it from <c>data-state</c>
/// (<c>on</c> or <c>off</c>) and <c>data-disabled</c>. Keep the label the same in both
/// states; the pressed state already says whether it's on. Toggling needs an interactive
/// render mode.
/// </remarks>
public partial class TogglePrimitive
{
    /// <summary>
    /// Whether the toggle is on.
    /// </summary>
    [Parameter]
    public bool Pressed { get; set; }

    /// <summary>
    /// Invoked with the new state when the person toggles it.
    /// </summary>
    [Parameter]
    public EventCallback<bool> PressedChanged { get; set; }

    /// <summary>
    /// Prevents the toggle from being focused or pressed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The toggle's label, usually an icon with an <c>aria-label</c>, or short text.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c> and <c>aria-label</c>, applied to the <c>&lt;button&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task ToggleAsync()
    {
        Pressed = !Pressed;
        await PressedChanged.InvokeAsync(Pressed);
    }
}
