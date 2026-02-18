using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Core.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Windows.Media.Imaging;

namespace admin.Features.Users;

public partial class UsersViewModel : BaseViewModel
{
    [ObservableProperty]
    private List<UserModel> _users = new();

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private string _searchUsername = string.Empty;

    [ObservableProperty]
    private int _totalPages = 1;

    private readonly IApiService _apiService;
    public UsersViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task FetchSearchUsers(string username, int page, int pageSize)
    {
        await HandleActionAsync(async () =>
        {
            var usersResponse = await _apiService.Profile.SearchProfiles(username, page, pageSize);
            Users = usersResponse.Profiles.Select(p => new UserModel(
                p.UserId,
                p.DisplayName,
                p.Username,
                null,
                p.LastOnline
            )).ToList();

            TotalPages = usersResponse.TotalPages;

            foreach (var user in Users)
            {
                user.Avatar = await FetchAvatarDataAsync(user.UserId);
            };

        }, showUserMessage: false);
    }
    private async Task<BitmapImage?> FetchAvatarDataAsync(int userId)
    {
        BitmapImage? returnValue = null;
        await HandleActionAsync(async () =>
        {
            using var response = await _apiService.Profile.GetAvatar(userId);
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();

            using var ms = new MemoryStream(bytes);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            bitmap.Freeze();

            returnValue = bitmap;
        }, false);
        return returnValue;
    }

    [RelayCommand]
    private async Task pageUpdated(int page)
    {
        await FetchSearchUsers(SearchUsername, page, 10);
    }
    [RelayCommand]
    private async Task search()
    {
        await FetchSearchUsers(SearchUsername, CurrentPage, 10);
    }
}
