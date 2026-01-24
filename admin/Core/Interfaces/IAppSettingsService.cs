using admin.Core.Model;

namespace admin.Core.Interfaces;

public interface IAppSettingsService
{
    void ResetToDefaults();
    ApplicationSettings Current { get; }
    event EventHandler<string?>? SettingsChanged;
    Task SaveSettingsAsync();
    void UpdateTheme();
}
