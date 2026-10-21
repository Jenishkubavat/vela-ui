// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// A section that a <see cref="CollapsibleTrigger"/> shows and hides, built on
/// <see cref="CollapsiblePrimitive"/>. Put the trigger and a <see cref="CollapsibleContent"/>
/// inside it.
/// </summary>
/// <remarks>
/// <para>
/// Like shadcn/ui's, it adds no styles of its own: lay it out with <see cref="Class"/>, and
/// style open and closed states with <c>data-[state=open]:</c>. To make the trigger look
/// like a button, give it <c>class="@Button.GetClasses(ButtonVariant.Ghost, ButtonSize.Icon)"</c>.
/// </para>
/// <para>
/// Bind the state with <c>@bind-Open</c>, or leave it unbound and it keeps its own. Opening
/// and closing need an interactive render mode.
/// </para>
/// </remarks>
public partial class Collapsible
{
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
    /// CSS classes for the root <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The trigger and content, plus anything that stays visible.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the root <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Keeping the new state here means an unbound collapsible stays open when its parent re-renders.
    private async Task OnOpenChanged(bool open)
    {
        Open = open;
        await OpenChanged.InvokeAsync(open);
    }
}
