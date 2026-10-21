// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// What an <see cref="ItemMedia"/> holds.
/// </summary>
public enum ItemMediaVariant
{
    /// <summary>
    /// Anything, such as an <see cref="Avatar"/>, left at its own size.
    /// </summary>
    Default,

    /// <summary>
    /// An icon, sized to 16px unless it has its own size.
    /// </summary>
    Icon,

    /// <summary>
    /// An image, cropped to a square that follows the item's size.
    /// </summary>
    Image,
}
