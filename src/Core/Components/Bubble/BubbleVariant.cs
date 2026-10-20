// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The fill of a <see cref="Bubble"/>.
/// </summary>
public enum BubbleVariant
{
    /// <summary>
    /// A text-coloured fill, for the person's own messages. The default.
    /// </summary>
    Default,

    /// <summary>
    /// A raised graphite fill with a hairline edge, for other people's messages.
    /// </summary>
    Secondary,

    /// <summary>
    /// A sunken fill, quieter than <see cref="Secondary"/>.
    /// </summary>
    Muted,

    /// <summary>
    /// The flame's faint wash, to set one message apart without spending the flame itself.
    /// </summary>
    Tinted,

    /// <summary>
    /// No fill, a hairline edge.
    /// </summary>
    Outline,

    /// <summary>
    /// No fill, edge, or padding: plain text, such as an assistant's long answer.
    /// </summary>
    Ghost,

    /// <summary>
    /// A faint red fill with red text, for a message that failed to send.
    /// </summary>
    Destructive,
}
