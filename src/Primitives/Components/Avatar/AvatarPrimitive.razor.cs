// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// An unstyled avatar: a photo from <see cref="AvatarPrimitiveImage"/>, with an
/// <see cref="AvatarPrimitiveFallback"/> (usually initials) until it loads or if it fails.
/// </summary>
/// <remarks>
/// <para>
/// The fallback stays rendered until the photo reports that it loaded, so stack the image on
/// top of it (for example with <c>position: absolute</c> on the image). Then the initials
/// show while the photo downloads and under static server rendering, where load events
/// never arrive. Removing a photo that fails to load needs an interactive render mode.
/// </para>
/// <para>
/// If the person's name is shown next to the avatar, leave the image's <c>alt</c> empty so
/// screen readers don't read the name twice. Otherwise put the name in <c>alt</c>.
/// </para>
/// </remarks>
public partial class AvatarPrimitive
{
    /// <summary>
    /// The image and fallback.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, such as <c>class</c>, applied to the root <c>&lt;span&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Where the photo is in loading. <see cref="AvatarImageStatus.Loading"/> until the image
    /// reports back, including when there's no image.
    /// </summary>
    public AvatarImageStatus ImageStatus { get; private set; } = AvatarImageStatus.Loading;

    internal void SetImageStatus(AvatarImageStatus status)
    {
        if (ImageStatus == status)
        {
            return;
        }

        ImageStatus = status;
        StateHasChanged();
    }
}
