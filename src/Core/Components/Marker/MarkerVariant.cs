// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The style of a <see cref="Marker"/>.
/// </summary>
public enum MarkerVariant
{
    /// <summary>
    /// Plain muted text. The default.
    /// </summary>
    Default,

    /// <summary>
    /// Centred text with a hairline on each side, such as a date between messages.
    /// </summary>
    Separator,

    /// <summary>
    /// Text with a hairline underneath, to start a section.
    /// </summary>
    Border,
}
