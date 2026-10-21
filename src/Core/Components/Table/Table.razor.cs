// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A native <c>&lt;table&gt;</c> for rows of work: 36px rows, hairline separators, and no
/// zebra stripes. It scrolls sideways inside its container when it's too wide.
/// </summary>
/// <remarks>
/// <para>
/// Build it from <see cref="TableHeader"/>, <see cref="TableBody"/>, <see cref="TableFooter"/>,
/// <see cref="TableRow"/>, <see cref="TableHead"/>, <see cref="TableCell"/>, and
/// <see cref="TableCaption"/>. Rows highlight on hover. Mark the row under the keyboard
/// cursor with <c>data-state="selected"</c> to tint it and put a small flame stop at its
/// start, and finished rows with <c>data-done</c> to print them in faint greyscale.
/// </para>
/// <para>
/// Set IDs in <c>font-mono text-caption text-fg-faint</c> and numbers in
/// <c>text-end tabular-nums text-fg-muted</c>, so the eye reads the task first. A
/// <see cref="TableCaption"/> names the table for screen readers. Keyboard row navigation
/// isn't built in; it's up to the page.
/// </para>
/// </remarks>
public partial class Table
{
    private const string Classes = "w-full caption-bottom border-collapse text-body-sm text-fg";

    /// <summary>
    /// Extra CSS classes for the <c>&lt;table&gt;</c>, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The caption, header, body, and footer.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the <c>&lt;table&gt;</c>, not its scrolling container.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
