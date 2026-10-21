// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Primitives;

/// <summary>
/// Which way a primitive lays out or runs, for primitives such as
/// <see cref="SeparatorPrimitive"/> and <see cref="RadioGroupPrimitive"/>.
/// </summary>
public enum Orientation
{
    /// <summary>
    /// Left to right (right to left in RTL layouts).
    /// </summary>
    Horizontal,

    /// <summary>
    /// Top to bottom.
    /// </summary>
    Vertical,
}
