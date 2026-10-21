// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A link to one page, styled as a ghost <see cref="Button"/>. The current page is outlined
/// and marked <c>aria-current="page"</c>.
/// </summary>
/// <remarks>
/// It's a plain <c>&lt;a&gt;</c>, so pass <c>href</c> and it works under static server
/// rendering and Blazor's enhanced navigation. A link without <c>href</c> can't be focused,
/// so on the first page leave out Previous, or render it without <c>href</c> and with
/// <c>aria-disabled="true"</c>.
/// </remarks>
public partial class PaginationLink
{
    /// <summary>
    /// Marks this link as the page being shown. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool IsActive { get; set; }

    /// <summary>
    /// The link's size, using the button sizes. Defaults to <see cref="ButtonSize.Icon"/>,
    /// a square that fits a page number.
    /// </summary>
    [Parameter]
    public ButtonSize Size { get; set; } = ButtonSize.Icon;

    /// <summary>
    /// Extra CSS classes, added after the button classes.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The link's content, usually the page number.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>href</c>, applied to the <c>&lt;a&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
