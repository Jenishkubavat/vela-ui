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
/// Escape closes it, and so does a click on the backdrop: one that both starts and ends
/// outside the dialog's box, so selecting text and dragging out doesn't close it, and the
/// click never reaches the page underneath. Clicks on the dialog's padding or scrollbar
/// count as inside.
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
    /// Closes the dialog when its backdrop is clicked. Defaults to <see langword="true"/>;
    /// an alert dialog that needs an answer turns it off. Read when the dialog opens.
    /// </summary>
    [Parameter]
    public bool CloseOnClickOutside { get; set; } = true;

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
        if (Root.Open)
        {
            await _module.InvokeVoidAsync("show", _dialog, CloseOnClickOutside);
        }
        else
        {
            await _module.InvokeVoidAsync("close", _dialog);
        }
    }

    // Escape and outside clicks close the dialog in the browser before Blazor hears of it,
    // so the browser's close event is what sets the state.
    private async Task OnCloseAsync()
    {
        _shown = false;
        await Root.SetOpenAsync(false);
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
