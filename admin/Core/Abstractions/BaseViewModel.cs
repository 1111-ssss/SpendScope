using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Abstractions.Controls;

namespace admin.Core.Abstractions;
public abstract class BaseViewModel : ObservableObject, INavigationAware
{
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
}
