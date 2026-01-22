using Refit;

namespace admin.Core.Interfaces;

public interface IErrorHandler
{
    Task HandleExceptionAsync(Exception ex, bool showUserMessage = true);
    Task HandleApiErrorAsync(ApiException ex, bool showUserMessage = true);
    Task ShowDialogAsync(string title, string message);
}