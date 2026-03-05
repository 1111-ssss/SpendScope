using admin.Core.Abstractions;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System.IO;
using System.Windows;

namespace admin.Features.UploadVersion;

public partial class UploadVersionViewModel : BaseViewModel
{
    private readonly List<string> _branches = new() { "Stable", "Dev" };
    public List<string> Branches => _branches;

    [ObservableProperty]
    private string _selectedBranch = "Stable";

    [ObservableProperty]
    private int _build = 1;

    [ObservableProperty]
    private string _changelog = string.Empty;

    private StreamPart? _streamPart;

    private readonly IApiService _apiService;
    public UploadVersionViewModel(
        IApiService apiService
    )
    {
        _apiService = apiService;
    }

    public override void OnNavigatedTo()
    {
        base.OnNavigatedTo();

        _ = GetLatestVersion();
    }

    public void DragDrop_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Copy;
    }

    public async Task DragDrop_UploadVersion(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] { Length: > 0 } files)
            return;

        string filePath = files[0];
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (extension != ".apk" && extension != ".ipa")
        {
            await ShowDialogAsync("Ошибка", "Поддерживаются только .apk и .ipa файлы");
            return;
        }

        string contentType = extension switch
        {
            ".apk" => "application/vnd.android.package-archive",
            ".ipa" => "application/octet-stream",
            _ => "application/octet-stream"
        };

        await HandleActionAsync(async () =>
        {
            FileStream? fileStream = null;
            try
            {
                fileStream = File.OpenRead(filePath);

                var streamPart = new StreamPart(
                    fileStream,
                    fileName: $"SpendScope{extension}",
                    contentType: contentType
                );

                _streamPart = streamPart;

                await ShowDialogAsync("Успех", "Файл готов к загрузке");
            }
            catch
            {
                fileStream?.Dispose();
                throw;
            }
        }, true, "Ошибка", "Не удалось получить файл");
    }

    private async Task GetLatestVersion()
    {
        await HandleActionAsync(async () =>
        {
            var result = await _apiService.Versions.GetLatest(SelectedBranch);
            if (result == null)
                return;

            Build = result.Build + 1;
        }, false);
    }

    async partial void OnSelectedBranchChanged(string value)
    {
        await GetLatestVersion();
    }

    [RelayCommand]
    private async Task UploadVersion()
    {
        await HandleActionAsync(async () =>
        {
            if (_streamPart == null || _streamPart.Value == null || _streamPart.Value.Length == 0)
                return;

            await _apiService.Versions.UploadVersion(
                SelectedBranch,
                Build,
                Changelog,
                _streamPart
            );
        });
    }
}
