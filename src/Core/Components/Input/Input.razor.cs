// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// A text input styled with the Vela UI theme tokens. Bind it with <c>@bind-Value</c>,
/// inside or outside an <see cref="EditForm"/>.
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
    private const string LayoutClasses =
        "h-8 w-full min-w-0 rounded-lg border px-2.5 py-1 text-base md:text-sm";

    private const string ColorClasses =
        "border-input bg-transparent placeholder:text-muted-foreground dark:bg-input/30";

    private const string MotionClasses =
        "transition-colors motion-reduce:transition-none";

    // Styles the "Choose file" button that browsers draw inside type="file" inputs.
    private const string FileButtonClasses =
        "file:inline-flex file:h-6 file:border-0 file:bg-transparent file:text-sm file:font-medium file:text-foreground";

    private const string FocusClasses =
        "outline-none focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/50";

    private const string InvalidClasses =
        "aria-invalid:border-destructive aria-invalid:ring-3 aria-invalid:ring-destructive/20 " +
        "dark:aria-invalid:border-destructive/50 dark:aria-invalid:ring-destructive/40";

    private const string DisabledClasses =
        "disabled:pointer-events-none disabled:cursor-not-allowed disabled:bg-input/50 disabled:opacity-50 " +
        "dark:disabled:bg-input/80";

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
