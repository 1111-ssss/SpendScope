using admin.Core.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;

namespace admin.Features.Profile;

public partial class ProfileViewModel : BaseViewModel
{
    [ObservableProperty]
    private BitmapImage? _userAvatar;

    [ObservableProperty]
    private string? _userDisplayName;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _userRole;

    public ProfileViewModel(
        
    )
    {
        UserDisplayName = "TEST";
    }
}