using admin.Core.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace admin.Features.Settings;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appTheme = string.Empty;

    [RelayCommand]
    void SetTheme(string theme)
    {

    }
}
