// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The dialog's title, an <c>&lt;h2&gt;</c> that gives the dialog its accessible name.
/// Every dialog needs one. Phrase it as the question or the task: "Archive 3 projects?".
/// </summary>
public sealed partial class DialogPrimitiveTitle
{
    [CascadingParameter]
    private DialogPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// The title text.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the <c>&lt;h2&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private DialogPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(DialogPrimitiveTitle)} must be inside a {nameof(DialogPrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
