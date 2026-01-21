using admin.Shell.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Refit;
using System.Net;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace admin.Core.Abstractions;
public abstract class BaseViewModel : ObservableObject, INavigationAware
{
    private IContentDialogService? _dialogService;
    protected IContentDialogService DialogService =>
        _dialogService ??= App.GetRequiredService<IContentDialogService>()
            ?? throw new InvalidOperationException("IContentDialogService не зарегистрирован в DI");
    private MainWindowViewModel? _mainVM;
    protected MainWindowViewModel MainWindowViewModel =>
        _mainVM ??= App.GetRequiredService<MainWindowViewModel>()
            ?? throw new InvalidOperationException("MainWindowViewMode не зарегистрирован в DI");
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

    protected async Task HandleHttpRequest(
        Func<Task> action,
        bool showErrMessage = true
    )
    {
        try
        {
            await action();
        }
        catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            MainWindowViewModel.NavigateToAuthWindow();

            await ShowApiErrorAsync(ex);
        }
        catch (ApiException ex) when (showErrMessage)
        {
            await ShowApiErrorAsync(ex);
        }
        catch (Exception ex) when (showErrMessage)
        {
            await ShowGenericErrorAsync(ex);
        }
    }
    protected virtual async Task ShowApiErrorAsync(ApiException ex)
    {
        string title = $"Ошибка: {ex.StatusCode}";
        string message = ex.Message;
        
        await DialogService.ShowAsync(new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "Закрыть",
        }, default);

        //await DialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        //{
        //    Title = title,
        //    Content = message,
        //    CloseButtonText = "Закрыть",
        //});
    }

    protected virtual async Task ShowGenericErrorAsync(Exception ex)
    {
        await DialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        {
            Title = "Неожиданная ошибка",
            Content = "Что-то пошло не так…\n\n" + ex.Message,
            CloseButtonText = "Закрыть",
        });
    }
}
