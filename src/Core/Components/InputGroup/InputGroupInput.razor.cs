// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// The text input inside an <see cref="InputGroup"/>. It has no edge or background of its
/// own; the group draws them, including focus and validation errors.
/// </summary>
/// <remarks>
/// It works like <see cref="Input"/>: bind it with <c>@bind-Value</c>, inside or outside an
/// <see cref="EditForm"/>, and <c>aria-invalid</c> is set when the bound field has errors.
/// </remarks>
public partial class InputGroupInput : InputBase<string?>
{
    // 16px text on small screens stops iOS from zooming into the field; 14px from md up.
    private const string Classes =
        "h-8 w-full min-w-0 flex-1 rounded-none bg-transparent px-3 text-base md:text-body text-fg " +
        "caret-flame-text placeholder:text-fg-faint outline-none disabled:cursor-not-allowed disabled:text-fg-faint";

    /// <summary>
    /// The HTML <c>type</c> attribute, such as <c>text</c>, <c>search</c>, <c>email</c>, or
    /// <c>url</c>. Defaults to <c>text</c>.
    /// </summary>
    [Parameter]
    public string Type { get; set; } = "text";

    /// <summary>
    /// Prevents the input from being focused or edited.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Updates the bound value on every keystroke instead of when the input loses focus.
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
    /// The rendered <c>&lt;input&gt;</c>, for calling <c>FocusAsync()</c>.
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
