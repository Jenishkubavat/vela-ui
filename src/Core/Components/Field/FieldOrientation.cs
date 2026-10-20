// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// How a <see cref="Field"/> arranges its label and control.
/// </summary>
public enum FieldOrientation
{
    /// <summary>
    /// Label above the control, with the description and error below. The default.
    /// </summary>
    Vertical,

    /// <summary>
    /// Label and control side by side. Suits checkboxes and switches.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Vertical in narrow containers, horizontal once the surrounding
    /// <see cref="FieldGroup"/> is wide enough.
    /// </summary>
    Responsive,
}
