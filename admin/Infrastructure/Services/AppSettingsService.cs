using admin.Core.Interfaces;
using admin.Core.Model;

namespace admin.Infrastructure.Services;

public class AppSettingsService : IAppSettingsService
{
    public ApplicationSettings Current { get; private set; }
    public event EventHandler<string?>? SettingsChanged;

    private readonly IStorageService _storageService;
    public AppSettingsService(IStorageService storageService)
    {
        _storageService = storageService;
        Current = LoadOrCreateDefault();
        Current.PropertyChanged += (sender, args) => SettingsChanged?.Invoke(this, args.PropertyName);
    }
    public void ResetToDefaults()
    {
        Current = new();
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
}
