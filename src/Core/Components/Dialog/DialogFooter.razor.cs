// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// The band along the bottom of a <see cref="DialogContent"/> that holds the actions:
/// Cancel first, then the primary action, aligned to the end. On narrow screens they stack,
/// primary on top.
/// </summary>
/// <remarks>
/// When the content has an <see cref="DialogContent.OnConfirm"/>, the footer also shows the
/// keys (<c>esc</c> to cancel, <c>Ctrl ↵</c> to confirm) from the small breakpoint up.
/// Keep the flame for the confirm action: one default <see cref="Button"/>, the rest outline
/// or ghost.
/// </remarks>
public partial class DialogFooter
{
    // It reaches out to the panel's edges, so its hairline and band run the full width.
    private const string LayoutClasses =
        "-mx-6 -mb-5 mt-2 flex flex-col-reverse gap-2 border-t px-6 py-3 sm:flex-row sm:items-center sm:justify-end";

    private const string ColorClasses = "bg-surface";

    // Shown only when the content handles Ctrl+Enter (data-confirm).
    private const string HintClasses =
        "me-auto hidden items-center gap-1.5 text-caption text-fg-faint group-data-confirm/dialog:sm:flex";

    /// <summary>
    /// Adds an outline Close button after the content. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The action buttons.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the footer's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
