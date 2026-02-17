using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Users;

public partial class UsersPage : INavigableView<UsersViewModel>
{
    public UsersViewModel ViewModel { get; }
    public UsersPage(UsersViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}
