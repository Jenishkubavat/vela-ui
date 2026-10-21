// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Cli.Install;

/// <summary>
/// A file a command is about to write. <see cref="Path"/> is relative to the project folder
/// and uses forward slashes.
/// </summary>
internal sealed record PlannedFile(string Path, byte[] Content);
