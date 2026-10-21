// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// Tells the primitives inside it which way text reads, so keyboard navigation follows
/// it: in right-to-left layouts the Left arrow means "next".
/// </summary>
/// <remarks>
/// It renders no element of its own, so also set <c>dir="rtl"</c> on <c>&lt;html&gt;</c>
/// (or a wrapper) for the browser's own layout. Components read the value with
/// <c>[CascadingParameter] public TextDirection Direction { get; set; }</c>; without a
/// provider they get <see cref="TextDirection.LeftToRight"/>.
/// </remarks>
public partial class DirectionPrimitive
{
    /// <summary>
    /// The reading direction. Defaults to <see cref="TextDirection.LeftToRight"/>.
    /// </summary>
    [Parameter]
    public TextDirection Direction { get; set; } = TextDirection.LeftToRight;

    /// <summary>
    /// The part of the app that reads in this direction.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
