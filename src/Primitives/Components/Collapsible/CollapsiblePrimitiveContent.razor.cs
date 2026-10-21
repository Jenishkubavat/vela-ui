// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The part of a <see cref="CollapsiblePrimitive"/> that's shown when it's open.
/// </summary>
/// <remarks>
/// It isn't rendered while closed, unless <see cref="ForceMount"/> keeps it in the page with
/// the <c>hidden</c> attribute. Its <c>id</c> is generated so the trigger can point to it;
/// one you pass is replaced.
/// </remarks>
public partial class CollapsiblePrimitiveContent
{
    [CascadingParameter]
    private CollapsiblePrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// Keeps the content in the page while closed, hidden with the <c>hidden</c> attribute,
    /// so the browser's find-in-page and search engines still see it. Defaults to
    /// <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool ForceMount { get; set; }

    /// <summary>
    /// The content to show and hide.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the content's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private CollapsiblePrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(CollapsiblePrimitiveContent)} must be inside a {nameof(CollapsiblePrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
