// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Vela.UI.Primitives;

/// <summary>
/// The photo in an <see cref="AvatarPrimitive"/>. It's removed if it fails to load, which
/// leaves the <see cref="AvatarPrimitiveFallback"/> showing.
/// </summary>
public partial class AvatarPrimitiveImage
{
    private bool _started;
    private string? _currentSrc;

    [CascadingParameter]
    private AvatarPrimitive? CascadedRoot { get; set; }

    /// <summary>
    /// The photo's URL. When it's empty, the avatar shows its fallback.
    /// </summary>
    [Parameter]
    public string? Src { get; set; }

    /// <summary>
    /// Invoked when the photo starts loading, loads, or fails. Requires an interactive render
    /// mode for anything after the first <see cref="AvatarImageStatus.Loading"/>.
    /// </summary>
    [Parameter]
    public EventCallback<AvatarImageStatus> OnLoadingStatusChange { get; set; }

    /// <summary>
    /// Other attributes, such as <c>alt</c> and <c>class</c>, applied to the <c>&lt;img&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private AvatarPrimitive Root => CascadedRoot
        ?? throw new InvalidOperationException($"{nameof(AvatarPrimitiveImage)} must be inside a {nameof(AvatarPrimitive)}.");

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        // A parent re-rendering with the same photo keeps its status; a new photo starts over.
        if (_started && _currentSrc == Src)
        {
            return;
        }

        _started = true;
        _currentSrc = Src;
        await SetStatusAsync(string.IsNullOrEmpty(Src) ? AvatarImageStatus.Error : AvatarImageStatus.Loading);
    }

    private async Task SetStatusAsync(AvatarImageStatus status)
    {
        Root.SetImageStatus(status);
        await OnLoadingStatusChange.InvokeAsync(status);
    }
}
