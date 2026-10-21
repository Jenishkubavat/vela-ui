// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// The browser's own <c>&lt;select&gt;</c>, styled as a Vela field, with
/// <see cref="NativeSelectOption"/> and <see cref="NativeSelectOptGroup"/> inside. Bind it
/// with <c>@bind-Value</c>; strings, numbers, and enums all work, like
/// <see cref="InputSelect{TValue}"/>.
/// </summary>
/// <remarks>
/// The list itself is drawn by the browser and operating system, so it's fully keyboard and
/// screen reader accessible and works on touch devices and in static server-rendered forms.
/// Use it for simple choices; a custom Select can come later for richer lists.
/// <c>Class</c> styles the wrapper; other attributes go on the <c>&lt;select&gt;</c>.
/// </remarks>
public partial class NativeSelect<TValue>
{
    private const string WrapperClasses = "group/native-select relative w-fit";

    // The select uses the same sunken well as Input; pe-8 leaves room for the chevron.
    private const string SelectClasses =
        "h-8 w-full min-w-0 cursor-pointer appearance-none rounded-md bg-surface-sunken ps-3 pe-8 text-base text-fg " +
        "shadow-control md:text-body not-disabled:hover:shadow-control-hover " +
        "transition-shadow duration-(--vela-quick) ease-vela motion-reduce:transition-none " +
        "outline-none focus-visible:shadow-control-focus focus-visible:outline-2 focus-visible:outline-offset-1 " +
        "focus-visible:outline-focus not-disabled:aria-invalid:shadow-control-invalid " +
        "disabled:cursor-not-allowed disabled:bg-transparent disabled:text-fg-faint disabled:shadow-hairline " +
        "data-[size=sm]:h-7 data-[size=sm]:ps-2.5 data-[size=sm]:pe-7 data-[size=sm]:text-body-sm";

    private const string ChevronClasses =
        "pointer-events-none absolute end-2.5 top-1/2 size-4 -translate-y-1/2 text-fg-faint select-none " +
        "group-data-[size=sm]/native-select:end-2 group-data-[size=sm]/native-select:size-3.5";

    /// <summary>
    /// The height. Defaults to <see cref="NativeSelectSize.Default"/>.
    /// </summary>
    [Parameter]
    public NativeSelectSize Size { get; set; } = NativeSelectSize.Default;

    /// <summary>
    /// Prevents the select from being focused or changed.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Extra CSS classes for the wrapper, such as a width (<c>w-full</c>).
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The options, as <see cref="NativeSelectOption"/> and <see cref="NativeSelectOptGroup"/> parts.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The rendered <c>&lt;select&gt;</c>, for calling <c>FocusAsync()</c>.
    /// </summary>
    public ElementReference? Element { get; protected set; }

    private string SizeName => Size == NativeSelectSize.Small ? "sm" : "default";

    private void SetValue(ChangeEventArgs args) => CurrentValueAsString = args.Value?.ToString();

    /// <inheritdoc />
    protected override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out TValue result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }

        validationErrorMessage = $"The {DisplayName ?? FieldIdentifier.FieldName} field is not valid.";
        return false;
    }
}
