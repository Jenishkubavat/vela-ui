// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// The multi-line field inside an <see cref="InputGroup"/>, for a prompt or message box with
/// actions below it. It grows with its content; the group draws its edge.
/// </summary>
/// <remarks>
/// It works like <see cref="Textarea"/>: bind it with <c>@bind-Value</c>, inside or outside
/// an <see cref="EditForm"/>, and <c>aria-invalid</c> is set when the bound field has errors.
/// </remarks>
public partial class InputGroupTextarea : InputBase<string?>
{
    private const string Classes =
        "flex field-sizing-content min-h-16 w-full min-w-0 flex-1 resize-none rounded-none bg-transparent px-3 py-2 " +
        "text-base md:text-body text-fg caret-flame-text placeholder:text-fg-faint outline-none " +
        "disabled:cursor-not-allowed disabled:text-fg-faint";

    /// <summary>
    /// Prevents the field from being focused or edited.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Updates the bound value on every keystroke instead of when the field loses focus.
    /// Each keystroke is a round trip under Interactive Server rendering, so leave it off
    /// for ordinary forms.
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The rendered <c>&lt;textarea&gt;</c>, for calling <c>FocusAsync()</c>.
    /// </summary>
    public ElementReference? Element { get; protected set; }

    // Only one of these has a handler, so Blazor attaches only that event to the element.
    private EventCallback<ChangeEventArgs> ChangeHandler =>
        Immediate ? default : EventCallback.Factory.Create<ChangeEventArgs>(this, SetValue);

    private EventCallback<ChangeEventArgs> InputHandler =>
        Immediate ? EventCallback.Factory.Create<ChangeEventArgs>(this, SetValue) : default;

    private void SetValue(ChangeEventArgs args) => CurrentValueAsString = args.Value?.ToString();

    /// <inheritdoc />
    protected override bool TryParseValueFromString(
        string? value,
        out string? result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
