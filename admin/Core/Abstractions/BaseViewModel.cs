using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Refit;
using Serilog;
using System.Net.Http;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace admin.Core.Abstractions;
public abstract class BaseViewModel : ObservableObject, INavigationAware
{
    private IErrorHandler? _errorHandler;
    protected IErrorHandler ErrorHandler =>
        _errorHandler ??= App.GetRequiredService<IErrorHandler>()
            ?? throw new InvalidOperationException("IErrorHandler не зарегистрирован в DI");
    public virtual Task OnNavigatedToAsync()
    {
        OnNavigatedTo();

        return Task.CompletedTask;
    }

    public virtual void OnNavigatedTo() { }

    public virtual Task OnNavigatedFromAsync()
    {
        OnNavigatedFrom();

        return Task.CompletedTask;
    }

    public virtual void OnNavigatedFrom() { }

    protected async Task HandleActionAsync(
        Func<Task> action,
        bool showUserMessage = true,
        string? messageTitle = null,
        string? messageText = null
    )
    {
        try
        {
            await action();
        }
        catch (ApiException ex)
        {
            await ErrorHandler.HandleApiErrorAsync(ex, showUserMessage, messageTitle, messageText);
        }
        catch (TaskCanceledException ex)
        {
            Log.Logger.Information($"Запрос был отменен по причине CancellationToken");
        }
        catch (Exception ex)
        {
            await ErrorHandler.HandleExceptionAsync(ex, showUserMessage, messageTitle, messageText);
        }
    }
}
