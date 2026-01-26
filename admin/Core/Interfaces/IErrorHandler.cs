using Refit;

namespace admin.Core.Interfaces;

public interface IErrorHandler
{
    Task HandleExceptionAsync(Exception ex, bool showUserMessage = true, string? messageTitle = null, string? messageText = null);
    Task HandleApiErrorAsync(ApiException ex, bool showUserMessage = true, string? messageTitle = null, string? messageText = null);
    Task ShowDialogAsync(string title, string message);
}