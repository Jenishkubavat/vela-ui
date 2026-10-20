// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// Lays out one form control with its <see cref="FieldLabel"/>, <see cref="FieldDescription"/>,
/// and <see cref="FieldError"/>.
/// </summary>
/// <remarks>
/// <para>
/// Field arranges its parts but doesn't connect them for assistive technology; give the
/// control an <c>id</c> and point the parts at it, so screen readers announce the label,
/// hint, and error with the control:
/// </para>
/// <code>
/// &lt;Field&gt;
///     &lt;FieldLabel for="email"&gt;Email&lt;/FieldLabel&gt;
///     &lt;Input id="email" aria-describedby="email-hint email-error" @bind-Value="model.Email" /&gt;
///     &lt;FieldDescription id="email-hint"&gt;We'll never share it.&lt;/FieldDescription&gt;
///     &lt;FieldError id="email-error" For="() =&gt; model.Email" /&gt;
/// &lt;/Field&gt;
/// </code>
/// <para>
/// Add <c>data-disabled="true"</c> to fade the label of a disabled control.
/// </para>
/// </remarks>
public partial class Field
{
    private const string LayoutClasses = "group/field flex w-full gap-1.5";

    /// <summary>
    /// How the label and control are arranged. Defaults to <see cref="FieldOrientation.Vertical"/>.
    /// </summary>
    [Parameter]
    public FieldOrientation Orientation { get; set; } = FieldOrientation.Vertical;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The field's parts: a label, a control, and optionally a description and an error.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>data-disabled</c>, applied to the field's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private static string OrientationClasses(FieldOrientation orientation) => orientation switch
    {
        FieldOrientation.Vertical =>
            "flex-col *:w-full [&>.sr-only]:w-auto",
        FieldOrientation.Horizontal =>
            "flex-row items-center has-[>[data-slot=field-content]]:items-start " +
            "*:data-[slot=field-label]:flex-auto",
        // Switches to a row once the surrounding FieldGroup container reaches the md width.
        FieldOrientation.Responsive =>
            "flex-col *:w-full [&>.sr-only]:w-auto " +
            "@md/field-group:flex-row @md/field-group:items-center @md/field-group:*:w-auto " +
            "@md/field-group:has-[>[data-slot=field-content]]:items-start " +
            "@md/field-group:*:data-[slot=field-label]:flex-auto",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string OrientationName(FieldOrientation orientation) => orientation switch
    {
        FieldOrientation.Vertical => "vertical",
        FieldOrientation.Horizontal => "horizontal",
        FieldOrientation.Responsive => "responsive",
        _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null),
    };
}
