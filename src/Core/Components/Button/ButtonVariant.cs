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
    /// Solid primary color. Use for the main action on a page or in a dialog.
    /// </summary>
    Default,

    /// <summary>
    /// Solid destructive color. Use for actions that delete data or can't be undone.
    /// </summary>
    Destructive,

    /// <summary>
    /// Bordered, with the page background.
    /// </summary>
    Outline,

    /// <summary>
    /// Muted solid color, for actions that sit next to a primary button.
    /// </summary>
    Secondary,

    /// <summary>
    /// No background until hovered. Use in toolbars and dense layouts.
    /// </summary>
    Ghost,

    /// <summary>
    /// Looks like a text link but keeps button semantics.
    /// </summary>
    Link,
}
