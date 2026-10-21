// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled box that keeps its content at a fixed width-to-height ratio as the width
/// changes, such as a 16:9 video frame or a square thumbnail.
/// </summary>
/// <remarks>
/// It sets the CSS <c>aspect-ratio</c> property on a <c>&lt;div&gt;</c> and keeps any
/// <c>style</c> you pass. Give it a width (it doesn't set one) and make the content fill
/// it, for example <c>&lt;img style="width:100%;height:100%;object-fit:cover"&gt;</c>.
/// </remarks>
public partial class AspectRatioPrimitive
{
    /// <summary>
    /// Width divided by height, such as <c>16.0 / 9</c>. Defaults to 1 (square).
    /// </summary>
    [Parameter]
    public double Ratio { get; set; } = 1;

    /// <summary>
    /// The content to size, usually an image or video.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c> or <c>style</c>, applied to the box's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private string StyleValue
    {
        get
        {
            // Invariant culture: a comma decimal separator (1,7778) would make the CSS value invalid.
            var ratio = $"aspect-ratio: {Ratio.ToString(CultureInfo.InvariantCulture)}";
            return AdditionalAttributes is not null
                && AdditionalAttributes.TryGetValue("style", out var style)
                && style is string userStyle
                && !string.IsNullOrWhiteSpace(userStyle)
                    ? $"{ratio}; {userStyle}"
                    : ratio;
        }
    }
}
