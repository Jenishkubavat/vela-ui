// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// How big an <see cref="Attachment"/> is.
/// </summary>
public enum AttachmentSize
{
    /// <summary>
    /// A 40px thumbnail and body text.
    /// </summary>
    Default,

    /// <summary>
    /// A 32px thumbnail and caption text.
    /// </summary>
    Small,

    /// <summary>
    /// A 28px thumbnail, caption text, and tighter rounding, for chips in a message box.
    /// </summary>
    ExtraSmall,
}
