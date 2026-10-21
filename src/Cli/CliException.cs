// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Cli;

/// <summary>
/// A problem the user can fix, such as a missing project or an unknown component. Its
/// message is printed as it is, so it should say what to do next.
/// </summary>
internal sealed class CliException(string message) : Exception(message);
