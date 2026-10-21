// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The look of an <see cref="Item"/>.
/// </summary>
public enum ItemVariant
{
    /// <summary>
    /// No edge or fill, for items in a list or menu.
    /// </summary>
    Default,

    /// <summary>
    /// A hairline edge, for an item that stands on its own.
    /// </summary>
    Outline,

    /// <summary>
    /// A faint fill, to set an item apart without an edge.
    /// </summary>
    Muted,
}
