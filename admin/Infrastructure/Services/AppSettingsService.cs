using admin.Core.Interfaces;
using admin.Core.Model;
using System.ComponentModel;
using Wpf.Ui.Appearance;

namespace admin.Infrastructure.Services;

public class AppSettingsService : IAppSettingsService
{
    private ApplicationSettings _current;
    public ApplicationSettings Current => _current;
    public event EventHandler<string?>? SettingsChanged;

    private readonly IStorageService _storageService;
    public AppSettingsService(IStorageService storageService)
    {
        _storageService = storageService;
        _current = LoadOrCreateDefault();
        _current.PropertyChanged += OnCurrentPropertyChanged;
    }

    public void UpdateTheme()
    {
        if (Enum.TryParse<ApplicationTheme>(Current.CurrentTheme, true, out ApplicationTheme theme))
            ApplicationThemeManager.Apply(theme);
    }
    public void ResetToDefaults()
    {
        if (_current != null)
            _current.PropertyChanged -= OnCurrentPropertyChanged;

        _current = new();
        _current.PropertyChanged += OnCurrentPropertyChanged;
        SettingsChanged?.Invoke(this, null);
    }
    private ApplicationSettings LoadOrCreateDefault()
    {
        var loaded = _storageService.LoadSettings();
        return loaded ?? new ApplicationSettings();
    }
    public async Task SaveSettingsAsync()
    {
        await _storageService.SaveSettingsAsync(Current);
    }
    private void OnCurrentPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SettingsChanged?.Invoke(this, e.PropertyName);
        _ = SaveSettingsAsync();
    }
}
