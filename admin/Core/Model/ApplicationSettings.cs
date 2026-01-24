using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Appearance;

namespace admin.Core.Model;

public partial class ApplicationSettings : ObservableObject
{
    [ObservableProperty]
    private string _serverBaseURI = "http://127.0.0.1:5012/api";

    [ObservableProperty]
    private ApplicationTheme _theme = ApplicationTheme.Light;

    [ObservableProperty]
    private string _savedUsername = string.Empty;

    [ObservableProperty]
    private bool _rememberUsername = true;

    [ObservableProperty]
    private List<string> _themes = new List<string>()
    {
        "Light",
        "Dark",
        "HighContrast",
    };

    [ObservableProperty]
    private string _currentTheme = "Dark";
}
