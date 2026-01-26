using admin.Core.Interfaces;
using admin.Core.Model;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net;
using System.Text.Json;
using Wpf.Ui;
using Wpf.Ui.Extensions;

namespace admin.Infrastructure.Services;

public class ErrorHandler : IErrorHandler
{
    private readonly IContentDialogService _dialogService;
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(IContentDialogService dialogService, ILogger<ErrorHandler> logger)
    {
        _dialogService = dialogService;
        _logger = logger;
    }

    public async Task HandleExceptionAsync(Exception ex, bool showUserMessage = true, string? messageTitle = null, string? messageText = null)
    {
        _logger.LogError(ex, "Неизвестная ошибка");

        if (!showUserMessage) return;

        string title = messageTitle ?? "Неожиданная ошибка";
        string message = messageText ?? "Что-то пошло не так…\n\n" + (ex.Message ?? "Неизвестная ошибка");

        if (ex is ApiException apiEx)
        {
            await HandleApiErrorAsync(apiEx, showUserMessage);
            return;
        }

        await ShowDialogAsync(title, message);
    }

    public async Task HandleApiErrorAsync(ApiException ex, bool showUserMessage = true, string? messageTitle = null, string? messageText = null)
    {
        _logger.LogWarning($"Ошибка API {ex.StatusCode}: {ex.Message}");

        if (!showUserMessage) return;

        string title = messageTitle ?? $"Ошибка API {((int)ex.StatusCode)}: {ex.StatusCode}";

        string content = ex.Content ?? string.Empty;
        if (!string.IsNullOrEmpty(content))
        {
            using JsonDocument doc = JsonDocument.Parse(content);
            content = doc.RootElement.TryGetProperty("error", out JsonElement errorElem) ?
                errorElem.ToString() ?? "Ошибка без описания" : "Неизвестная ошибка";
        }

        string message = messageText ?? content ?? ex.Message ?? "Сервер вернул ошибку";

        if (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            message = "Сессия истекла или доступ запрещён.\nПожалуйста, войдите заново.";
            title = "Авторизация требуется";
        }

        await ShowDialogAsync(title, message);
    }

    public async Task ShowDialogAsync(string title, string message)
    {
        //await _dialogService.ShowAsync(new ContentDialog
        //{
        //    Title = title,
        //    Content = message,
        //    CloseButtonText = "Закрыть",
        //    DefaultButton = ContentDialogButton.Close,
        //}, CancellationToken.None);

        await _dialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        {
            Title = title,
            Content = message,
            CloseButtonText = "Закрыть",
        });
    }
}
