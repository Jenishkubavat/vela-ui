// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Demo.Components.Docs;

/// <summary>
/// One row of an API table: a component parameter, its type and default, and its summary.
/// </summary>
internal sealed record ApiParameter(string Name, string Type, string Default, MarkupString Description, bool Required);
