// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Vela.UI.Components;

/// <summary>
/// The error under a control, in red 12px text with <c>role="alert"</c> so screen readers
/// announce it when it appears. It renders nothing while there is no error.
/// </summary>
/// <remarks>
/// <para>
/// The message comes from, in order: <see cref="ChildContent"/>, <see cref="Errors"/>, or
/// the <see cref="EditForm"/> validation messages for <see cref="For"/>. Repeated messages
/// show once; several become a list.
/// </para>
/// <para>
/// Give it an <c>id</c> and list that id in the control's <c>aria-describedby</c> so the
/// error is read with the control. Say what's wrong and how to fix it ("That email is
/// missing a domain, like @company.com"), not just "Invalid".
/// </para>
/// </remarks>
public sealed partial class FieldError : IDisposable
{
    private const string Classes = "text-caption text-danger";

    private EditContext? _subscribedContext;
    private FieldIdentifier _field;

    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    /// <summary>
    /// The bound field to show <see cref="EditForm"/> validation messages for, written the
    /// same way as for Blazor's <c>ValidationMessage</c>: <c>For="() =&gt; model.Email"</c>.
    /// Requires a surrounding <see cref="EditForm"/>. The error updates as validation runs.
    /// </summary>
    [Parameter]
    public Expression<Func<object?>>? For { get; set; }

    /// <summary>
    /// Messages to show when you validate without an <see cref="EditForm"/>.
    /// </summary>
    [Parameter]
    public IEnumerable<string>? Errors { get; set; }

    /// <summary>
    /// Extra CSS classes, added after the defaults.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// A custom error, shown instead of <see cref="Errors"/> or the validation messages.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Other attributes, most importantly <c>id</c>, applied to the error's <c>&lt;div&gt;</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (For is null)
        {
            Subscribe(null);
            return;
        }

        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(FieldError)} with a {nameof(For)} parameter must be inside an {nameof(EditForm)}.");
        }

        _field = FieldIdentifier.Create(For);
        Subscribe(CurrentEditContext);
    }

    /// <summary>
    /// Stops listening for validation changes.
    /// </summary>
    public void Dispose() => Subscribe(null);

    private List<string> GetMessages()
    {
        var messages = Errors
            ?? (_subscribedContext is not null ? _subscribedContext.GetValidationMessages(_field) : []);

        return messages.Where(message => !string.IsNullOrWhiteSpace(message)).Distinct().ToList();
    }

    private void Subscribe(EditContext? context)
    {
        if (context == _subscribedContext)
        {
            return;
        }

        if (_subscribedContext is not null)
        {
            _subscribedContext.OnValidationStateChanged -= OnValidationStateChanged;
        }

        _subscribedContext = context;

        if (_subscribedContext is not null)
        {
            _subscribedContext.OnValidationStateChanged += OnValidationStateChanged;
        }
    }

    private void OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs args) =>
        StateHasChanged();
}
