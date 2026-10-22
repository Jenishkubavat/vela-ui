// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// A modal dialog that interrupts only when a decision is needed. Built on
/// <see cref="DialogPrimitive"/>; put a <see cref="DialogTrigger"/> and a
/// <see cref="DialogContent"/> inside it.
/// </summary>
/// <remarks>
/// <para>
/// The dialog is a native <c>&lt;dialog&gt;</c>, so the page behind becomes inert, Escape
/// and a click on the dimmed backdrop close it, focus moves in when it opens and back to the
/// trigger when it closes. Phrase the <see cref="DialogTitle"/> as the question ("Archive 3
/// projects?"), say in the <see cref="DialogDescription"/> what will happen and how to undo
/// it, and prefer an Undo over a dialog when the action can be taken back.
/// </para>
/// <para>
/// Bind the state with <c>@bind-Open</c>, or leave it unbound and it keeps its own. Opening
/// needs an interactive render mode. The root renders no element, so it takes no classes.
/// Unlike shadcn/ui there are no <c>DialogPortal</c> or <c>DialogOverlay</c> parts: the
/// dialog sits in the browser's top layer, and the backdrop is its <c>::backdrop</c>,
/// styled from <see cref="DialogContent"/> with <c>backdrop:</c> utilities.
/// </para>
/// </remarks>
public partial class Dialog
{
    /// <summary>
    /// Whether the dialog is open. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Invoked with the new state when the dialog opens or closes, including on Escape and
    /// outside clicks.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// The trigger and the content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    // Keeping the new state here means an unbound dialog stays open when its parent re-renders.
    private async Task OnOpenChanged(bool open)
    {
        Open = open;
        await OpenChanged.InvokeAsync(open);
    }
}
