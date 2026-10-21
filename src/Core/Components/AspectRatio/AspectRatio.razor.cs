// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Vela.UI.Primitives;

namespace Vela.UI.Components;

/// <summary>
/// Keeps its content at a fixed width-to-height ratio as the width changes, such as a
/// 16:9 video frame or a square thumbnail. Styled on top of <see cref="AspectRatioPrimitive"/>.
/// </summary>
/// <remarks>
/// It fills the available width. Make the content fill it too, for example
/// <c>&lt;img class="size-full object-cover"&gt;</c>.
/// </remarks>
public partial class AspectRatio
{
    private const string Classes = "relative w-full overflow-hidden";

    /// <summary>
    /// Width divided by height, such as <c>16.0 / 9</c>. Defaults to 1 (square).
    /// </summary>
    [Parameter]
    public double Ratio { get; set; } = 1;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The content to size, usually an image or video.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the container's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
