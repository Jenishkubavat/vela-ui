// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The height and padding of a <see cref="Button"/>. The <c>Icon</c> sizes are square and
/// meant for a single icon; give those buttons an accessible name with <c>aria-label</c>,
/// because they have no visible text.
/// </summary>
public enum ButtonSize
{
    /// <summary>
    /// Standard height for forms and page actions.
    /// </summary>
    Default,

    /// <summary>
    /// Smallest text button, for tight spaces such as table rows and badges.
    /// </summary>
    ExtraSmall,

    /// <summary>
    /// Compact height for dense layouts.
    /// </summary>
    Small,

    /// <summary>
    /// Taller button for prominent actions.
    /// </summary>
    Large,

    /// <summary>
    /// Square, matching the height of <see cref="Default"/>.
    /// </summary>
    Icon,

    /// <summary>
    /// Square, matching the height of <see cref="ExtraSmall"/>.
    /// </summary>
    IconExtraSmall,

    /// <summary>
    /// Square, matching the height of <see cref="Small"/>.
    /// </summary>
    IconSmall,

    /// <summary>
    /// Square, matching the height of <see cref="Large"/>.
    /// </summary>
    IconLarge,
}
