// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// Where an <see cref="Attachment"/> is in being added.
/// </summary>
public enum AttachmentState
{
    /// <summary>
    /// Nothing chosen yet: an empty slot with a dashed edge, waiting for a file.
    /// </summary>
    Idle,

    /// <summary>
    /// The file is being sent. The media dims and the title pulses.
    /// </summary>
    Uploading,

    /// <summary>
    /// The file arrived and is being scanned, converted, or indexed.
    /// </summary>
    Processing,

    /// <summary>
    /// The upload or processing failed. The edge and media turn to the danger colour.
    /// </summary>
    Error,

    /// <summary>
    /// The file is ready.
    /// </summary>
    Done,
}
