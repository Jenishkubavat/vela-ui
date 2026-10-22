// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// A sentence under the title that says what will happen, read by screen readers when the
/// dialog opens. Optional: without it, the dialog has no <c>aria-describedby</c>.
/// </summary>
public sealed partial class DialogPrimitiveDescription
{
    [CascadingParameter]
    private DialogPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// The description text.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the <c>&lt;p&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private DialogPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(DialogPrimitiveDescription)} must be inside a {nameof(DialogPrimitive)}.");

    /// <inheritdoc />
    protected override void OnInitialized() => Root.SetHasDescription(true);

    /// <inheritdoc />
    public void Dispose() => CascadedRoot?.SetHasDescription(false);
}
