// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Vela.UI.Primitives;

/// <summary>
/// The dialog itself: a native <c>&lt;dialog&gt;</c> that a <see cref="DialogPrimitive"/>
/// opens with <c>showModal()</c>. It's named by its <see cref="DialogPrimitiveTitle"/> and
/// described by its <see cref="DialogPrimitiveDescription"/>.
/// </summary>
/// <remarks>
/// <para>
/// Pressing Escape or pressing the pointer down on the backdrop closes it. A press that
/// starts inside the dialog never does, even if it ends outside (for example when selecting
/// text). Presses on the <c>&lt;dialog&gt;</c> element's own padding count as outside, so
/// give it no padding and pad your content instead; browsers give it padding by default.
/// </para>
/// <para>
/// When it opens, the browser moves focus to the first element marked <c>autofocus</c>, or
/// else to the first focusable element (in some browsers, the dialog itself). Put a close
/// button last, so focus doesn't start on it. Style the backdrop with the <c>::backdrop</c>
/// pseudo-element and the dialog from <c>data-state</c> or <c>[open]</c>. Its <c>id</c> is
/// generated so the trigger can point to it; one you pass is replaced.
/// </para>
/// </remarks>
public sealed partial class DialogPrimitiveContent
{
    private const string ModulePath = "./_content/Vela.UI.Primitives/dialog.js";

    private ElementReference _dialog;
    private IJSObjectReference? _module;
    private bool _shown;

    [CascadingParameter]
    private DialogPrimitive? CascadedRoot { get; set; }

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Closes the dialog when the pointer is pressed on its backdrop. Defaults to
    /// <see langword="true"/>; an alert dialog that needs an answer turns it off.
    /// </summary>
    [Parameter]
    public bool CloseOnPointerDownOutside { get; set; } = true;

    /// <summary>
    /// The dialog's content: a title, an optional description, the body, and its actions.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the <c>&lt;dialog&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private DialogPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(DialogPrimitiveContent)} must be inside a {nameof(DialogPrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;

    // The browser shows the dialog, so after each render bring it in line with the state.
    // Under prerendering this never runs, so nothing is imported until the page is interactive.
    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!RendererInfo.IsInteractive || Root.Open == _shown)
        {
            return;
        }

        _module ??= await JS.InvokeAsync<IJSObjectReference>("import", ModulePath);
        _shown = Root.Open;
        await _module.InvokeVoidAsync(Root.Open ? "show" : "close", _dialog);
    }

    // Escape closes the dialog in the browser before Blazor hears of it, so the browser's
    // close event is what sets the state.
    private async Task OnCloseAsync()
    {
        _shown = false;
        await Root.SetOpenAsync(false);
    }

    private async Task OnPointerDownOutsideAsync()
    {
        if (CloseOnPointerDownOutside)
        {
            await Root.SetOpenAsync(false);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_module is null)
        {
            return;
        }

        try
        {
            await _module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
            // The circuit is already gone, and the page with it.
        }
    }
}
