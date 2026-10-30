// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;

namespace Vela.UI.Demo.Components.Docs;

/// <summary>
/// Reads a component's parameters, their defaults, and their XML documentation, so the API
/// tables come from the source and can't drift from it.
/// </summary>
internal static partial class ApiDocs
{
    private static readonly ConcurrentDictionary<Assembly, IReadOnlyDictionary<string, string>> Docs = new();

    // Parameters inherited from Blazor's InputBase have no docs in our XML file.
    private static readonly Dictionary<string, string> InheritedDocs = new()
    {
        ["Value"] = "The value. Bind it with <code>@bind-Value</code>.",
        ["ValueChanged"] = "Set by <code>@bind-Value</code>.",
        ["ValueExpression"] = "Set by <code>@bind-Value</code>; it tells validation which field this is.",
        ["DisplayName"] = "The field's name in validation messages.",
        ["AdditionalAttributes"] = "Other attributes, passed to the element.",
    };

    /// <summary>
    /// The <see cref="ParameterAttribute"/> properties of <paramref name="component"/>, with
    /// <c>AdditionalAttributes</c> last.
    /// </summary>
    public static IReadOnlyList<ApiParameter> GetParameters(Type component)
    {
        var defaults = Activator.CreateInstance(component);
        var nullability = new NullabilityInfoContext();
        return component.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetCustomAttribute<ParameterAttribute>() is not null)
            .OrderBy(property => property.Name == "AdditionalAttributes")
            .Select(property => new ApiParameter(
                property.Name,
                TypeName(property.PropertyType, nullability.Create(property)),
                DefaultValue(property.GetValue(defaults)),
                Describe(property),
                property.GetCustomAttribute<EditorRequiredAttribute>() is not null))
            .ToList();
    }

    private static MarkupString Describe(PropertyInfo property)
    {
        var docs = Docs.GetOrAdd(property.DeclaringType!.Assembly, Load);
        var key = $"P:{property.DeclaringType.FullName}.{property.Name}";
        return new MarkupString(
            docs.GetValueOrDefault(key) ?? InheritedDocs.GetValueOrDefault(property.Name) ?? "");
    }

    // The XML file sits next to the assembly when the project generates one; other
    // assemblies (Blazor's own) have none here.
    private static IReadOnlyDictionary<string, string> Load(Assembly assembly)
    {
        var path = Path.ChangeExtension(assembly.Location, ".xml");
        if (!File.Exists(path))
        {
            return new Dictionary<string, string>();
        }

        return XDocument.Load(path).Descendants("member")
            .Where(member => member.Element("summary") is not null)
            .ToDictionary(member => (string)member.Attribute("name")!, member => ToHtml(member.Element("summary")!));
    }

    // Turns a <summary> into HTML: code and references become <code>, and the rest is
    // encoded text with its whitespace collapsed.
    private static string ToHtml(XElement summary)
    {
        var html = new StringBuilder();
        foreach (var node in summary.Nodes())
        {
            html.Append(node switch
            {
                XText text => WebUtility.HtmlEncode(text.Value),
                XElement { Name.LocalName: "c" } code => Code(code.Value),
                XElement { Name.LocalName: "see" } see => Code(ShortName(
                    (string?)see.Attribute("cref") ?? (string?)see.Attribute("langword") ?? see.Value)),
                XElement { Name.LocalName: "paramref" or "typeparamref" } reference => Code((string?)reference.Attribute("name") ?? ""),
                XElement element => " " + ToHtml(element) + " ",
                _ => "",
            });
        }

        return Whitespace().Replace(html.ToString(), " ").Trim();
    }

    private static string Code(string text) => $"<code>{WebUtility.HtmlEncode(text)}</code>";

    // "T:Vela.UI.Components.Button" becomes "Button", "M:...Button.GetClasses(...)" becomes
    // "Button.GetClasses".
    private static string ShortName(string cref)
    {
        var name = cref.Contains(':') ? cref[(cref.IndexOf(':') + 1)..] : cref;
        name = name.Split('(')[0].Split('`')[0];
        var parts = name.Split('.');
        return cref.StartsWith("T:", StringComparison.Ordinal) || parts.Length < 2
            ? parts[^1]
            : $"{parts[^2]}.{parts[^1]}";
    }

    private static string TypeName(Type type, NullabilityInfo nullability)
    {
        if (Nullable.GetUnderlyingType(type) is { } underlying)
        {
            return TypeName(underlying, nullability) + "?";
        }

        var name = type switch
        {
            _ when type == typeof(bool) => "bool",
            _ when type == typeof(string) => "string",
            _ when type == typeof(int) => "int",
            _ when type == typeof(double) => "double",
            _ when type == typeof(object) => "object",
            { IsGenericType: true } => $"{type.Name.Split('`')[0]}<{string.Join(", ",
                type.GetGenericArguments().Select((argument, index) => TypeName(argument,
                    index < nullability.GenericTypeArguments.Length ? nullability.GenericTypeArguments[index] : nullability)))}>",
            _ => type.Name,
        };

        return !type.IsValueType && nullability.ReadState == NullabilityState.Nullable ? name + "?" : name;
    }

    private static string DefaultValue(object? value) => value switch
    {
        null => "none",
        bool flag => flag ? "true" : "false",
        string text => $"\"{text}\"",
        Enum option => $"{option.GetType().Name}.{option}",
        IFormattable number => number.ToString(null, CultureInfo.InvariantCulture),
        _ when value.GetType().Name.StartsWith("EventCallback", StringComparison.Ordinal) => "none",
        _ => value.ToString() ?? "none",
    };

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
