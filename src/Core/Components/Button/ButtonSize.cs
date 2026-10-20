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
    /// 32px, the compact default for forms, toolbars, and page actions.
    /// </summary>
    Default,

    /// <summary>
    /// 24px, for tight spaces such as table rows.
    /// </summary>
    ExtraSmall,

    /// <summary>
    /// 28px, for dense layouts.
    /// </summary>
    Small,

    /// <summary>
    /// 40px, for empty states and onboarding.
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
