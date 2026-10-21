// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Vela.UI.Cli.Registry;

/// <summary>
/// A file bundled in the registry. <see cref="Path"/> is relative to the folder it's
/// installed into and always uses forward slashes.
/// </summary>
internal sealed record RegistryFile(string Path, byte[] Content);
