// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The sliding thumb inside a <see cref="SwitchPrimitive"/>. It carries the switch's
/// <c>data-state</c> so you can move it when the switch is on.
/// </summary>
public partial class SwitchPrimitiveThumb
{
    [CascadingParameter]
    private SwitchPrimitive? Switch { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the thumb's <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
