// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// Which side of the conversation a <see cref="Message"/> sits on.
/// </summary>
public enum MessageAlign
{
    /// <summary>
    /// The start side, usually for other people's messages. The default.
    /// </summary>
    Start,

    /// <summary>
    /// The end side, usually for the person's own messages. The avatar moves to that side too.
    /// </summary>
    End,
}
