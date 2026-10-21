// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled section that a <see cref="CollapsiblePrimitiveTrigger"/> shows and hides.
/// Put the trigger and a <see cref="CollapsiblePrimitiveContent"/> inside it.
/// </summary>
/// <remarks>
/// Bind the state with <c>@bind-Open</c>; without a binding it keeps its own, starting from
/// <see cref="Open"/>. The root, trigger, and content all get <c>data-state</c>
/// (<c>open</c> or <c>closed</c>) and <c>data-disabled</c> for styling. Opening and closing
/// need an interactive render mode; under static server rendering the section stays as it
/// was first rendered.
/// </remarks>
public partial class CollapsiblePrimitive
{
    private readonly string _contentId = $"collapsible-{Guid.NewGuid():N}";

    /// <summary>
    /// Whether the content is shown. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Invoked with the new state when the person opens or closes the section.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Prevents the trigger from being focused or pressed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// The trigger and content, plus anything that stays visible.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the root <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// The state for styling: <c>open</c> or <c>closed</c>.
    /// </summary>
    public string State => Open ? "open" : "closed";

    /// <summary>
    /// The <c>id</c> the content renders with, which the trigger points to through
    /// <c>aria-controls</c>.
    /// </summary>
    public string ContentId => _contentId;

    internal async Task ToggleAsync()
    {
        Open = !Open;
        await OpenChanged.InvokeAsync(Open);
        StateHasChanged();
    }
}
