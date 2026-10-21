// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// What an <see cref="AvatarPrimitive"/> shows until its photo loads, or instead of it:
/// usually the person's initials. It's removed once the photo has loaded.
/// </summary>
public partial class AvatarPrimitiveFallback
{
    [CascadingParameter]
    private AvatarPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// The fallback content, such as initials or an icon.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the fallback's <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private AvatarPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(AvatarPrimitiveFallback)} must be inside a {nameof(AvatarPrimitive)}.");

    /// <inheritdoc />
    protected override void OnParametersSet() => _ = Root;
}
