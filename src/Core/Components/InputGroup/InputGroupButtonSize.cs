// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// How big an <see cref="InputGroupButton"/> is. Each maps to the matching
/// <see cref="ButtonSize"/>.
/// </summary>
public enum InputGroupButtonSize
{
    /// <summary>
    /// 24px tall, with a label. Fits inside a single-line group.
    /// </summary>
    ExtraSmall,

    /// <summary>
    /// 28px tall, with a label. For addons above or below the control.
    /// </summary>
    Small,

    /// <summary>
    /// A 24px square for an icon.
    /// </summary>
    IconExtraSmall,

    /// <summary>
    /// A 28px square for an icon.
    /// </summary>
    IconSmall,
}
