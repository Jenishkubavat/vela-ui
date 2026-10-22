// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled modal dialog. Put a <see cref="DialogPrimitiveTrigger"/> and a
/// <see cref="DialogPrimitiveContent"/> inside it; the content holds a
/// <see cref="DialogPrimitiveTitle"/>, an optional <see cref="DialogPrimitiveDescription"/>,
/// and usually a <see cref="DialogPrimitiveClose"/>.
/// </summary>
/// <remarks>
/// <para>
/// The content is a native <c>&lt;dialog&gt;</c> opened with <c>showModal()</c>, so the
/// browser handles the hard parts: the rest of the page becomes inert (it can't be focused,
/// clicked, or read by screen readers), the dialog sits above everything else, Escape closes
/// it, focus moves into it when it opens, and focus returns to the trigger when it closes.
/// </para>
/// <para>
/// Bind the state with <c>@bind-Open</c>, or leave it unbound and it keeps its own. The
/// root renders no element. Opening needs an interactive render mode; under static server
/// rendering the dialog stays closed. A modal dialog doesn't stop the page behind it from
/// scrolling: add <c>html:has(dialog[open]:modal) { overflow: hidden; }</c> to your CSS.
/// </para>
/// </remarks>
public partial class DialogPrimitive
{
    private readonly string _id = $"dialog-{Guid.NewGuid():N}";

    /// <summary>
    /// Whether the dialog is open. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Invoked with the new state when the dialog opens or closes, including when the person
    /// presses Escape or clicks outside it.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// The trigger and the content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The state for styling: <c>open</c> or <c>closed</c>.
    /// </summary>
    public string State => Open ? "open" : "closed";

    /// <summary>
    /// The <c>id</c> the <c>&lt;dialog&gt;</c> renders with, which the trigger points to.
    /// </summary>
    public string ContentId => _id;

    /// <summary>
    /// The <c>id</c> of the title, which names the dialog.
    /// </summary>
    public string TitleId => $"{_id}-title";

    /// <summary>
    /// The <c>id</c> of the description, which describes the dialog when there is one.
    /// </summary>
    public string DescriptionId => $"{_id}-description";

    internal bool HasDescription { get; private set; }

    internal async Task SetOpenAsync(bool open)
    {
        if (Open == open)
        {
            return;
        }

        Open = open;
        await OpenChanged.InvokeAsync(open);
        StateHasChanged();
    }

    // The content renders before its description, so it re-renders once the description
    // exists, to point aria-describedby at it.
    internal void SetHasDescription(bool hasDescription)
    {
        if (HasDescription != hasDescription)
        {
            HasDescription = hasDescription;
            StateHasChanged();
        }
    }
}
