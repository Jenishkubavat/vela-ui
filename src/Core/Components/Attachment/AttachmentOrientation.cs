// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// How an <see cref="Attachment"/> lays out its thumbnail and text.
/// </summary>
public enum AttachmentOrientation
{
    /// <summary>
    /// The thumbnail beside the text, as a row.
    /// </summary>
    Horizontal,

    /// <summary>
    /// The thumbnail above the text, as a tile, with actions over its corner.
    /// </summary>
    Vertical,
}
