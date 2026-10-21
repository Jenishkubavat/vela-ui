// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Primitives;

/// <summary>
/// Shared handling for attributes passed to primitives.
/// </summary>
internal static class PrimitiveAttributes
{
    /// <summary>
    /// Splits attributes for a primitive that draws a visible box around a hidden native
    /// control: <c>class</c>, <c>style</c>, and <c>data-*</c> style the box; everything else
    /// (<c>id</c>, <c>name</c>, <c>aria-*</c>, <c>required</c>, ...) belongs on the control so
    /// labels, forms, and screen readers work.
    /// </summary>
    public static (Dictionary<string, object> Root, Dictionary<string, object> Control) SplitForBox(
        IReadOnlyDictionary<string, object>? attributes)
    {
        var root = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        var control = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        if (attributes is not null)
        {
            foreach (var (name, value) in attributes)
            {
                var stylesTheBox = name.Equals("class", StringComparison.OrdinalIgnoreCase)
                    || name.Equals("style", StringComparison.OrdinalIgnoreCase)
                    || name.StartsWith("data-", StringComparison.OrdinalIgnoreCase);
                (stylesTheBox ? root : control)[name] = value;
            }
        }

        return (root, control);
    }

    /// <summary>
    /// Puts a primitive's required inline style first and keeps the caller's own style after it.
    /// </summary>
    public static string MergeStyle(string requiredStyle, IReadOnlyDictionary<string, object>? attributes) =>
        attributes is not null
            && attributes.TryGetValue("style", out var style)
            && style is string userStyle
            && !string.IsNullOrWhiteSpace(userStyle)
                ? $"{requiredStyle}; {userStyle}"
                : requiredStyle;
}
