// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Components;

/// <summary>
/// Builds a <c>class</c> attribute value from several groups of CSS classes.
/// </summary>
public static class ClassNames
{
    /// <summary>
    /// Joins the non-empty values with a single space, in the order given.
    /// </summary>
    /// <remarks>
    /// Classes are appended, not merged. When two Tailwind utilities set the same
    /// property (for example <c>h-9</c> and <c>h-12</c>), the stylesheet decides which
    /// one wins, not their order here. To override a default, use Tailwind's important
    /// modifier (<c>h-12!</c>) or edit the component's classes.
    /// </remarks>
    public static string Join(params string?[] values) =>
        string.Join(' ', values.Where(value => !string.IsNullOrWhiteSpace(value)).Select(value => value!.Trim()));
}
