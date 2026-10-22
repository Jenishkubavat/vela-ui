// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// A native <c>&lt;button&gt;</c> that closes its <see cref="DialogPrimitive"/>, such as
/// Cancel or an icon-only close button (give that one an <c>aria-label</c>).
/// </summary>
public sealed partial class DialogPrimitiveClose
{
    [CascadingParameter]
    private DialogPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// The button's label.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c> and <c>aria-label</c>, applied to the <c>&lt;button&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private DialogPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(DialogPrimitiveClose)} must be inside a {nameof(DialogPrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
