// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The native <c>&lt;button&gt;</c> that opens a <see cref="DialogPrimitive"/>. Focus
/// returns to it when the dialog closes.
/// </summary>
public sealed partial class DialogPrimitiveTrigger
{
    [CascadingParameter]
    private DialogPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// The button's label.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the <c>&lt;button&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private DialogPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(DialogPrimitiveTrigger)} must be inside a {nameof(DialogPrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
