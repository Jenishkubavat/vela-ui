// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The filled part of a <see cref="ProgressPrimitive"/>. It sets no size of its own: give it
/// a width or transform from the bar's <see cref="ProgressPrimitive.Percentage"/>, for example
/// <c>style="width: 40%"</c>.
/// </summary>
public partial class ProgressPrimitiveIndicator
{
    [CascadingParameter]
    private ProgressPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c> and <c>style</c>, applied to the indicator's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private ProgressPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(ProgressPrimitiveIndicator)} must be inside a {nameof(ProgressPrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
