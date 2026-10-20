// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The visual style of a <see cref="Button"/>.
/// </summary>
public enum ButtonVariant
{
    /// <summary>
    /// The primary action, filled with the flame. Use one per view: if two buttons compete
    /// for the flame, one of them isn't primary.
    /// </summary>
    Default,

    /// <summary>
    /// Quiet red text until hovered, then fills red. For actions that delete data or can't
    /// be undone, so they never shout before you reach for them.
    /// </summary>
    Destructive,

    /// <summary>
    /// A neutral edge on a transparent background, lighter than <see cref="Secondary"/>.
    /// </summary>
    Outline,

    /// <summary>
    /// A raised graphite key with a hairline edge. The usual choice for actions that
    /// aren't the primary one.
    /// </summary>
    Secondary,

    /// <summary>
    /// Muted text until hovered. For the many small actions in toolbars and rows.
    /// </summary>
    Ghost,

    /// <summary>
    /// Underlined text that keeps button semantics.
    /// </summary>
    Link,
}
