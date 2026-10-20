// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// A text input styled as a Vela sunken well. Bind it with <c>@bind-Value</c>, inside or
/// outside an <see cref="EditForm"/>.
/// </summary>
/// <remarks>
/// <para>
/// Built on Blazor's <see cref="InputBase{TValue}"/>, so it takes part in
/// <see cref="EditContext"/> validation and static server-rendered form posts like
/// <see cref="InputText"/> does. When the bound field has validation errors,
/// <see cref="InputBase{TValue}"/> adds <c>aria-invalid="true"</c>, which turns on the
/// error styles and tells screen readers. Outside a form, set <c>aria-invalid</c> yourself.
/// </para>
/// <para>
/// Give it an <c>id</c> and point a <see cref="Label"/> at it with <c>for</c> so the
/// input has an accessible name. For numbers, dates, or files, use Blazor's
/// <see cref="InputNumber{TValue}"/>, <see cref="InputDate{TValue}"/>, or
/// <see cref="InputFile"/> with <c>class="@Input.GetClasses()"</c>.
/// </para>
/// </remarks>
public partial class Input : InputBase<string?>
{
    // 16px text on small screens stops iOS from zooming into the field; 14px from md up.
    private const string LayoutClasses =
        "h-8 w-full min-w-0 rounded-md px-3 text-base md:text-body";

    // A sunken well with a strong hairline ring and a flame-coloured caret.
    private const string ColorClasses =
        "bg-surface-sunken text-fg shadow-control caret-flame-text placeholder:text-fg-faint " +
        "not-read-only:hover:shadow-control-hover";

    private const string MotionClasses =
        "transition-shadow duration-(--vela-quick) ease-vela motion-reduce:transition-none";

    // Styles the "Choose file" button that browsers draw inside type="file" inputs.
    private const string FileButtonClasses =
        "file:me-3 file:inline-flex file:h-6 file:border-0 file:bg-transparent file:text-label file:text-fg";

    // The edge turns to the focus colour and the 2px ring sits just outside it.
    private const string FocusClasses =
        "outline-none focus-visible:shadow-control-focus " +
        "focus-visible:outline-2 focus-visible:outline-offset-1 focus-visible:outline-focus";

    private const string InvalidClasses =
        "not-disabled:aria-invalid:shadow-control-invalid";

    // read-only: alone also matches disabled inputs, so pair it with enabled:.
    private const string ReadOnlyClasses =
        "read-only:enabled:bg-transparent read-only:enabled:shadow-hairline " +
        "read-only:enabled:font-mono read-only:enabled:text-code read-only:enabled:text-fg-muted";

    private const string DisabledClasses =
        "disabled:cursor-not-allowed disabled:bg-transparent disabled:text-fg-faint disabled:shadow-hairline";

    /// <summary>
    /// The HTML <c>type</c> attribute, such as <c>text</c>, <c>email</c>, <c>password</c>,
    /// <c>search</c>, <c>tel</c>, or <c>url</c>. Defaults to <c>text</c>.
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
    /// Useful for search boxes and live previews. Each keystroke is a round trip to the
    /// server under Interactive Server rendering, so leave it off for ordinary forms.
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults. To replace a default, use Tailwind's
    /// important modifier, for example <c>Class="h-10!"</c>.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The rendered <c>&lt;input&gt;</c>, for calling <c>FocusAsync()</c>.
    /// </summary>
    public ElementReference? Element { get; protected set; }

    /// <summary>
    /// Returns the classes an <see cref="Input"/> uses, so another input element can match
    /// it: <c>&lt;InputNumber class="@Input.GetClasses()" @bind-Value="..." /&gt;</c>.
    /// </summary>
    public static string GetClasses(string? additionalClasses = null) =>
        ClassNames.Join(
            LayoutClasses,
            ColorClasses,
            MotionClasses,
            FileButtonClasses,
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
