// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Primitives;

/// <summary>
/// Where an <see cref="AvatarPrimitiveImage"/> is in loading its photo.
/// </summary>
public enum AvatarImageStatus
{
    /// <summary>
    /// The browser is still fetching the photo, or hasn't reported back yet.
    /// </summary>
    Loading,

    /// <summary>
    /// The photo loaded and is showing.
    /// </summary>
    Loaded,

    /// <summary>
    /// There's no photo, or it failed to load.
    /// </summary>
    Error,
}
