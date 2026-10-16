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
/// </remarks>
public partial class Card
{
    private const string LayoutClasses =
        "group/card flex flex-col gap-(--card-spacing) overflow-hidden py-(--card-spacing) text-sm";

    private const string ColorClasses =
        "rounded-xl bg-card text-card-foreground ring-1 ring-foreground/10";

    // Sets the spacing every card part uses. Small is chosen through data-size.
    private const string SpacingClasses =
        "[--card-spacing:--spacing(4)] data-[size=sm]:[--card-spacing:--spacing(3)]";

    // A CardFooter brings its own padding and background, so drop the card's bottom padding.
    private const string FooterClasses =
        "has-data-[slot=card-footer]:pb-0 data-[size=sm]:has-data-[slot=card-footer]:pb-0";

    private const string ImageClasses =
        "has-[>img:first-child]:pt-0 *:[img:first-child]:rounded-t-xl *:[img:last-child]:rounded-b-xl";

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
