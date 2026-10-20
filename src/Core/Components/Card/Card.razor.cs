// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Components;

/// <summary>
/// A bordered container for related content, built from <see cref="CardHeader"/>,
/// <see cref="CardTitle"/>, <see cref="CardDescription"/>, <see cref="CardAction"/>,
/// <see cref="CardContent"/>, and <see cref="CardFooter"/>. Use only the parts you need.
/// </summary>
/// <remarks>
/// <para>
/// Every part's padding comes from the <c>--card-spacing</c> CSS variable, so one change
/// respaces the whole card: <c>Class="[--card-spacing:--spacing(6)]"</c>. To run content
/// edge to edge while staying aligned, give it <c>-mx-(--card-spacing)</c>.
/// </para>
/// <para>
/// An <c>&lt;img&gt;</c> placed first or last sits flush with the card's edge and takes its
/// rounded corners. Cards render no interactive behavior, so they work under static
/// server rendering.
/// </para>
/// <para>
/// Add <c>data-interactive</c> to a card that opens something: it lifts 1px on hover and
/// draws the focus ring. Keep a single link or button inside it as the real target. Add
/// <c>data-state="selected"</c> to mark the selected card with a 1px flame edge.
/// </para>
/// </remarks>
public partial class Card
{
    private const string LayoutClasses =
        "group/card flex flex-col gap-(--card-spacing) overflow-hidden py-(--card-spacing) text-body-sm";

    // The work surface with a hairline ring and, in Night, a 1px top highlight. No drop shadow at rest.
    private const string ColorClasses =
        "rounded-lg bg-surface text-fg shadow-card";

    // Sets the spacing every card part uses. Small is chosen through data-size.
    private const string SpacingClasses =
        "[--card-spacing:--spacing(5)] data-[size=sm]:[--card-spacing:--spacing(3)]";

    private const string StateClasses =
        "outline-none transition-[box-shadow,transform] duration-(--vela-settle) ease-vela motion-reduce:transition-none " +
        "data-interactive:cursor-pointer data-interactive:hover:-translate-y-px data-interactive:hover:shadow-card-hover " +
        "focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-focus " +
        "data-[state=selected]:shadow-glow data-[state=selected]:hover:shadow-glow";

    // A CardFooter brings its own padding and background, so drop the card's bottom padding.
    private const string FooterClasses =
        "has-data-[slot=card-footer]:pb-0 data-[size=sm]:has-data-[slot=card-footer]:pb-0";

    private const string ImageClasses =
        "has-[>img:first-child]:pt-0 *:[img:first-child]:rounded-t-lg *:[img:last-child]:rounded-b-lg";

    /// <summary>
    /// The spacing inside the card. Defaults to <see cref="CardSize.Default"/>.
    /// </summary>
    [Parameter]
    public CardSize Size { get; set; } = CardSize.Default;

    /// <summary>
    /// Extra CSS classes, added after the defaults. To replace a default, use Tailwind's
    /// important modifier, for example <c>Class="rounded-none!"</c>.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// The card's parts, such as <see cref="CardHeader"/> and <see cref="CardContent"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes applied to the card's root <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    // Uses shadcn/ui's names so selectors written for shadcn work unchanged.
    private static string SizeName(CardSize size) => size switch
    {
        CardSize.Default => "default",
        CardSize.Small => "sm",
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null),
    };
}
