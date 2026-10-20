// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// The visual style of an <see cref="Alert"/>.
/// </summary>
public enum AlertVariant
{
    /// <summary>
    /// A quiet panel with a muted icon. The default.
    /// </summary>
    Default,

    /// <summary>
    /// A red edge and a red icon, for errors that block the person until they act.
    /// The text stays readable in the normal text colour.
    /// </summary>
    Destructive,
}
