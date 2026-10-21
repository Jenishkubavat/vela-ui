// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The native <c>&lt;button&gt;</c> that opens and closes a <see cref="CollapsiblePrimitive"/>.
/// </summary>
/// <remarks>
/// Enter and Space press it. It carries <c>aria-expanded</c> and <c>aria-controls</c>, so
/// screen readers announce whether the section is open. Its label should name the section
/// ("Show 3 more repositories"), not the action alone.
/// </remarks>
public partial class CollapsiblePrimitiveTrigger
{
    [CascadingParameter]
    private CollapsiblePrimitive? CascadedRoot { get; set; }

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

    private CollapsiblePrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(CollapsiblePrimitiveTrigger)} must be inside a {nameof(CollapsiblePrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
