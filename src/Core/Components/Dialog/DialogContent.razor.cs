// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// The dialog panel: a raised surface over a dimmed, blurred room, with a close button in
/// its corner. Put a <see cref="DialogHeader"/>, the body, and a <see cref="DialogFooter"/>
/// inside it.
/// </summary>
/// <remarks>
/// <para>
/// When it opens, focus moves to the first element marked <c>autofocus</c>, or else to the
/// first focusable element; the close button comes last, so focus doesn't start on it. Set
/// <see cref="OnConfirm"/> to let Ctrl+Enter (⌘+Enter on a Mac) confirm, as the design system
/// asks; the footer then shows the <c>esc</c> and <c>Ctrl ↵</c> hints. Each key press inside
/// the dialog then reaches the server under Interactive Server rendering, so set it only
/// when the dialog has a confirm action.
/// </para>
/// <para>
/// The panel rises 8px into place when it opens, unless the person prefers reduced motion.
/// Restyle the backdrop with <c>backdrop:</c> utilities in <see cref="Class"/>. The page
/// behind stops scrolling through a rule in <c>vela.css</c>.
/// </para>
/// </remarks>
public partial class DialogContent
{
    // A <dialog> must keep its display unset while closed, or it would show, so the grid
    // only applies when it's open. group/dialog lets the parts follow the close button and
    // data-confirm.
    private const string LayoutClasses =
        "group/dialog m-auto w-full max-w-[min(var(--vela-dialog-width),calc(100%-2rem))] max-h-[calc(100%-2rem)] " +
        "overflow-y-auto rounded-xl border-0 p-6 pb-5 outline-none open:grid open:gap-4";

    private const string ColorClasses =
        "bg-surface-raised text-body text-fg shadow-overlay";

    private const string BackdropClasses =
        "backdrop:bg-scrim backdrop:backdrop-blur-[2px]";

    private const string MotionClasses =
        "motion-safe:open:animate-[vela-rise_var(--vela-settle)_var(--vela-ease)]";

    private static readonly string CloseButtonClasses =
        Button.GetClasses(ButtonVariant.Ghost, ButtonSize.IconSmall, "absolute top-3 end-3");

    /// <summary>
    /// Shows the close button (an X) in the top corner. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>
    /// Closes the dialog when its backdrop is clicked. Defaults to <see langword="true"/>;
    /// turn it off for a dialog that needs an answer.
    /// </summary>
    [Parameter]
    public bool CloseOnClickOutside { get; set; } = true;

    /// <summary>
    /// Invoked when the person presses Ctrl+Enter or ⌘+Enter inside the dialog, usually the
    /// same action as the footer's primary button. Requires an interactive render mode.
    /// </summary>
    [Parameter]
    public EventCallback OnConfirm { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The header, body, and footer.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the <c>&lt;dialog&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Without OnConfirm there's no handler at all, so key presses stay in the browser.
    private EventCallback<KeyboardEventArgs> KeyDownHandler =>
        OnConfirm.HasDelegate ? EventCallback.Factory.Create<KeyboardEventArgs>(this, OnKeyDownAsync) : default;

    private async Task OnKeyDownAsync(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" && (args.CtrlKey || args.MetaKey))
        {
            await OnConfirm.InvokeAsync();
        }
    }
}
