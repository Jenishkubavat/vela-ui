// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Vela.UI.Cli.Install;

/// <summary>
/// Writes planned files into a project without silently replacing anything.
/// </summary>
/// <remarks>
/// A file that already exists with the same content is left alone. If any file exists with
/// different content, nothing is written and the conflicts are listed, unless the user passed
/// <c>--overwrite</c>; then each replaced file is first copied to <c>.vela/backups/&lt;time&gt;/</c>.
/// </remarks>
internal static partial class FileInstaller
{
    /// <summary>
    /// Writes the files and returns the exit code: <see cref="VelaCli.Success"/>, or
    /// <see cref="VelaCli.Conflict"/> when files differ and <paramref name="overwrite"/> is off.
    /// </summary>
    public static int Install(
        string projectDirectory,
        IReadOnlyList<PlannedFile> files,
        bool overwrite,
        TextWriter output,
        TextWriter error)
    {
        var changed = files.Where(file => Exists(projectDirectory, file) && !SameContent(projectDirectory, file)).ToArray();
        if (changed.Length > 0 && !overwrite)
        {
            error.WriteLine("These files already exist and are different, so nothing was written:");
            foreach (var file in changed)
            {
                error.WriteLine($"  {file.Path}");
            }

            error.WriteLine("Run again with --overwrite to replace them. The current versions are kept in .vela/backups.");
            return VelaCli.Conflict;
        }

        if (changed.Length > 0)
        {
            var backup = Path.Combine(".vela", "backups", DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture));
            foreach (var file in changed)
            {
                var target = Path.Combine(projectDirectory, backup, file.Path);
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(FullPath(projectDirectory, file), target, overwrite: true);
            }

            output.WriteLine($"Replaced {changed.Length} changed file(s); the previous versions are in {backup.Replace('\\', '/')}.");
        }

        var toWrite = files.Where(file => !Exists(projectDirectory, file) || changed.Contains(file)).ToArray();
        foreach (var file in toWrite)
        {
            var target = FullPath(projectDirectory, file);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.WriteAllBytes(target, file.Content);
            output.WriteLine($"  {file.Path}");
        }

        if (toWrite.Length == 0)
        {
            output.WriteLine("  Every file is already up to date.");
        }

        return VelaCli.Success;
    }

    /// <summary>
    /// Moves a source file from the registry's namespace into <paramref name="targetNamespace"/>.
    /// Only the <c>@namespace</c> and <c>namespace</c> declarations change.
    /// </summary>
    public static byte[] RewriteNamespace(byte[] content, string targetNamespace)
    {
        var text = Encoding.UTF8.GetString(content);
        text = RazorNamespace().Replace(text, $"@namespace {targetNamespace}");
        text = CSharpNamespace().Replace(text, $"namespace {targetNamespace};");
        return Encoding.UTF8.GetBytes(text);
    }

    private static string FullPath(string projectDirectory, PlannedFile file) =>
        Path.Combine(projectDirectory, file.Path.Replace('/', Path.DirectorySeparatorChar));

    private static bool Exists(string projectDirectory, PlannedFile file) => File.Exists(FullPath(projectDirectory, file));

    private static bool SameContent(string projectDirectory, PlannedFile file) =>
        File.ReadAllBytes(FullPath(projectDirectory, file)).AsSpan().SequenceEqual(file.Content);

    [GeneratedRegex(@"^@namespace Vela\.UI\.Components\b(?=\s*$)", RegexOptions.Multiline)]
    private static partial Regex RazorNamespace();

    [GeneratedRegex(@"^namespace Vela\.UI\.Components;", RegexOptions.Multiline)]
    private static partial Regex CSharpNamespace();
}
