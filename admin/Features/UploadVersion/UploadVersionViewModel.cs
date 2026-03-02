using admin.Core.Abstractions;
using admin.Core.DTO.Versions.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System.IO;
using System.Windows;

namespace admin.Features.UploadVersion;

public partial class UploadVersionViewModel : BaseViewModel
{
    public readonly List<string> Branches = new() { "Stable", "Dev" };

    [ObservableProperty]
    private string _selectedBranch = "Stable";

    [ObservableProperty]
    private string _build = "0";

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

    private async void DragDrop_UploadVersion(object sender, DragEventArgs e)
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
            await using var fileStream = File.OpenRead(filePath);

            var streamPart = new StreamPart(
                fileStream,
                //fileName: Path.GetFileName(filePath),
                fileName: $"SpendScope{extension}",
                contentType: contentType,
                name: $"SpendScope{extension}"
            );

            _streamPart = streamPart;
        }, true, "Ошибка", "Не удалось получить файл");
    }

    private async Task<AppVersionResponse> GetLatestVersion()
    {
        return await HandleActionAsync(Task<AppVersionResponse> () =>
        {
            return await _apiService.Versions.GetLatest(SelectedBranch);
        }, false);
    }

    async partial void OnSelectedBranchChanged(string value)
    {
        var result = await GetLatestVersion();
        Build = (result.Build + 1).ToString();
    }

    [RelayCommand]
    private async Task UploadVersion()
    {
        await HandleActionAsync(async () =>
        {
            if (!int.TryParse(Build, out int build))
                return;
            if (_streamPart == null || _streamPart.Value == null || _streamPart.Value.Length == 0)
                return;

            await _apiService.Versions.UploadVersion(
                SelectedBranch,
                build,
                Changelog,
                _streamPart
            );
        });
    }
}
