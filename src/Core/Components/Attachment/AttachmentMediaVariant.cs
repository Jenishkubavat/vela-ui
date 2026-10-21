// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// What an <see cref="AttachmentMedia"/> shows.
/// </summary>
public enum AttachmentMediaVariant
{
    /// <summary>
    /// A file-type icon or a <see cref="Spinner"/> on a sunken tile.
    /// </summary>
    Icon,

    /// <summary>
    /// A preview image, cropped to a square and dimmed until the file is ready.
    /// </summary>
    Image,
}
