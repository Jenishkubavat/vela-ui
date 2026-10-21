// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled on/off switch for a setting that takes effect immediately: a track with a
/// native checkbox (<c>role="switch"</c>) stretched invisibly over it, and a
/// <see cref="SwitchPrimitiveThumb"/> inside.
/// </summary>
/// <remarks>
/// The track gets <c>data-state</c> (<c>checked</c> or <c>unchecked</c>) and
/// <c>data-disabled</c>. <c>class</c>, <c>style</c>, and <c>data-*</c> attributes style the
/// track; everything else (<c>id</c>, <c>name</c>, <c>aria-label</c>, ...) goes on the native
/// input. Space toggles it. For a choice that waits for a Save button, use a checkbox.
/// </remarks>
public partial class SwitchPrimitive
{
}
