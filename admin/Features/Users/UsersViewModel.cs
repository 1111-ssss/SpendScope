using admin.Core.Abstractions;
using admin.Core.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace admin.Features.Users;

public partial class UsersViewModel : BaseViewModel
{
    [ObservableProperty]
    private List<UserModel> _users = new();

    public UsersViewModel()
    {

    }
}
