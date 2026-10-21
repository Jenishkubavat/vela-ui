// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled checkbox: a box with a native checkbox stretched invisibly over it, and a
/// <see cref="CheckboxPrimitiveIndicator"/> inside for the tick.
/// </summary>
/// <remarks>
/// <para>
/// The box gets <c>data-state</c> (<c>checked</c>, <c>unchecked</c>, or <c>indeterminate</c>)
/// and <c>data-disabled</c>. <c>class</c>, <c>style</c>, and <c>data-*</c> attributes style
/// the box; every other attribute (<c>id</c>, <c>name</c>, <c>aria-label</c>,
/// <c>required</c>, ...) goes on the native input, so give the input's <c>id</c> to your
/// label's <c>for</c>.
/// </para>
/// <para>
/// Space toggles it. It works in static server-rendered forms; there, style the box with
/// <c>:has(:checked)</c>, because <c>data-state</c> is only updated by an interactive
/// render mode.
/// </para>
/// </remarks>
public partial class CheckboxPrimitive
{
    /// <summary>
    /// Shows the checkbox as partly checked, for a "select all" box whose items are only
    /// partly selected. Screen readers hear "mixed". Clicking it checks it; recompute this
    /// from the items afterwards.
    /// </summary>
    [Parameter]
    public bool Indeterminate { get; set; }

    /// <inheritdoc />
    public override string State => Indeterminate ? "indeterminate" : base.State;
}
