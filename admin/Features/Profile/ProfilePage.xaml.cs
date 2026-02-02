using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Profile;

public partial class ProfilePage : INavigableView<ProfileViewModel>
{
    public ProfileViewModel ViewModel { get; }
    public ProfilePage(ProfileViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}