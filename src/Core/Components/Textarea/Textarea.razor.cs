// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// A multi-line text field styled as a Vela sunken well. Bind it with <c>@bind-Value</c>,
/// inside or outside an <see cref="EditForm"/>.
/// </summary>
/// <remarks>
/// Built on Blazor's <see cref="InputBase{TValue}"/> like <see cref="InputTextArea"/>, so
/// validation errors add <c>aria-invalid="true"</c> and turn on the error styles. It grows
/// with its content (<c>field-sizing: content</c>) where the browser supports it, from a
/// 64px minimum; set <c>rows</c> or a height class for a fixed size. Give it an <c>id</c>
/// and point a <see cref="Label"/> at it.
/// </remarks>
public partial class Textarea : InputBase<string?>
{
    // 16px text on small screens stops iOS from zooming into the field; 14px from md up.
    private const string LayoutClasses =
        "flex field-sizing-content min-h-16 w-full min-w-0 rounded-md px-3 py-2 text-base md:text-body";

    // A sunken well with a strong hairline ring and a flame-coloured caret.
    private const string ColorClasses =
        "bg-surface-sunken text-fg shadow-control caret-flame-text placeholder:text-fg-faint " +
        "not-read-only:hover:shadow-control-hover";

    private const string MotionClasses =
        "transition-shadow duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    // The edge turns to the focus colour and the 2px ring sits just outside it.
    private const string FocusClasses =
        "outline-none focus-visible:shadow-control-focus " +
        "focus-visible:outline-2 focus-visible:outline-offset-1 focus-visible:outline-focus";

    private const string InvalidClasses = "not-disabled:aria-invalid:shadow-control-invalid";

    // read-only: alone also matches disabled fields, so pair it with enabled:.
    private const string ReadOnlyClasses =
        "read-only:enabled:bg-transparent read-only:enabled:shadow-hairline read-only:enabled:text-fg-muted";

    private const string DisabledClasses =
        "disabled:cursor-not-allowed disabled:bg-transparent disabled:text-fg-faint disabled:shadow-hairline";

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
    /// Extra CSS classes, added after the defaults. To replace a default, use Tailwind's
    /// important modifier, for example <c>Class="min-h-32!"</c>.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The rendered <c>&lt;textarea&gt;</c>, for calling <c>FocusAsync()</c>.
    /// </summary>
    public ElementReference? Element { get; protected set; }

    /// <summary>
    /// Returns the classes a <see cref="Textarea"/> uses, so another field can match it.
    /// </summary>
    public static string GetClasses(string? additionalClasses = null) =>
        ClassNames.Join(
            LayoutClasses,
            ColorClasses,
            MotionClasses,
            FocusClasses,
            InvalidClasses,
            ReadOnlyClasses,
            DisabledClasses,
            additionalClasses);

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
