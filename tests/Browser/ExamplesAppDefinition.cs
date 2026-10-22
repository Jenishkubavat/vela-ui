// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Browser.Tests;

/// <summary>
/// Shares one running examples app and one browser between the test classes in it.
/// </summary>
[CollectionDefinition(Name)]
public sealed class ExamplesAppDefinition : ICollectionFixture<ExamplesApp>
{
    /// <summary>
    /// The collection's name, for <c>[Collection]</c>.
    /// </summary>
    public const string Name = "Examples app";
}
