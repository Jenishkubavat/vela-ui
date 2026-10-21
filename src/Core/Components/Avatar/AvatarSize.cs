// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// How big an <see cref="Avatar"/> is.
/// </summary>
public enum AvatarSize
{
    /// <summary>
    /// 22px, for lists, tables, and owner fields.
    /// </summary>
    Default,

    /// <summary>
    /// 18px, for dense rows and inline mentions.
    /// </summary>
    Small,

    /// <summary>
    /// 32px, for headers and profile cards.
    /// </summary>
    Large,
}
