// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// The caption of a <see cref="FieldSet"/>. It must be the fieldset's first child: screen
/// readers announce it with every control inside.
/// </summary>
public partial class FieldLegend
{
    private const string Classes =
        "mb-1.5 text-fg data-[variant=label]:text-label data-[variant=legend]:text-title-sm";

    /// <summary>
    /// How prominent the legend is. Defaults to <see cref="FieldLegendVariant.Legend"/>.
    /// </summary>
    [Parameter]
    public FieldLegendVariant Variant { get; set; } = FieldLegendVariant.Legend;

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The legend text.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the <c>&lt;legend&gt;</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string VariantName(FieldLegendVariant variant) => variant switch
    {
        FieldLegendVariant.Legend => "legend",
        FieldLegendVariant.Label => "label",
        _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null),
    };
}
