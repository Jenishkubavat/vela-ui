// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The visual style of a <see cref="Badge"/>. Every badge is a calm pill: colour goes to
/// the word (or a status dot), never to a large fill.
/// </summary>
public enum BadgeVariant
{
    /// <summary>
    /// Text-coloured word with a hairline edge. The default.
    /// </summary>
    Default,

    /// <summary>
    /// Muted word on a raised fill, for secondary labels.
    /// </summary>
    Secondary,

    /// <summary>
    /// Red word with a hairline edge, for errors and blocked states that must not be missed.
    /// </summary>
    Destructive,

    /// <summary>
    /// Muted word with a hairline edge, the neutral status badge.
    /// </summary>
    Outline,

    /// <summary>
    /// Muted word with no edge until hovered.
    /// </summary>
    Ghost,

    /// <summary>
    /// Underlined word, for badges that are links.
    /// </summary>
    Link,
}
