using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Core.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Refit;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace admin.Features.Users;

public partial class UsersViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<UserModel> _users = new();

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private string _searchUsername = string.Empty;

    [ObservableProperty]
    private int _totalPages = 1;

    [ObservableProperty]
    private string? _userDisplayName = string.Empty;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private BitmapImage? _userAvatar;

    private readonly IApiService _apiService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UsersViewModel> _logger;

    public UsersViewModel(
        IApiService apiService,
        ICurrentUserService currentUserService,
        ILogger<UsersViewModel> logger
    )
    {
        _apiService = apiService;
        _currentUserService = currentUserService;
        _logger = logger;
        _ = InitUserProfile();
    }

    private async Task InitUserProfile()
    {
        if (int.TryParse(_currentUserService.UserId, out var userId))
            UserAvatar = await FetchAvatarDataAsync(userId);

        Username = string.IsNullOrEmpty(_currentUserService.UserName) ? "@username" : $"@{_currentUserService.UserName}";
    }

    public async Task FetchSearchUsers(string username, int page, int pageSize)
    {
        await HandleActionAsync(async () =>
        {
            var usersResponse = await _apiService.Profile.SearchProfiles(username, page, pageSize);
            if (usersResponse == null)
                return;

            var usersList = usersResponse.Profiles.Select(p => new UserModel(
                p.UserId,
                p.DisplayName,
                p.Username,
                null,
                p.LastOnline
            )).ToList();

            Users = new ObservableCollection<UserModel>(usersList);
            TotalPages = usersResponse.TotalPages;

            var avatarTasks = Users.Select(async user =>
                user.Avatar = await FetchAvatarDataAsync(user.UserId));

            await Task.WhenAll(avatarTasks);
        }, showUserMessage: false);
    }
    private async Task<BitmapImage?> FetchAvatarDataAsync(int userId)
    {
        try
        {
            using var response = await _apiService.Profile.GetAvatar(userId);
            response.EnsureSuccessStatusCode();

            using var ms = new MemoryStream(
                await response.Content.ReadAsByteArrayAsync()
            );
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
        catch (ApiException ex)
        {
            _logger.LogWarning($"Ошибка API {ex.StatusCode}: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Ошибка: {ex.Message}");
        }
        return null;
    }

    [RelayCommand]
    private async Task PageUpdated(int page)
    {
        await FetchSearchUsers(SearchUsername, page, 10);
    }
    [RelayCommand]
    private async Task Search()
    {
        await FetchSearchUsers(SearchUsername, CurrentPage, 10);
    }
}
