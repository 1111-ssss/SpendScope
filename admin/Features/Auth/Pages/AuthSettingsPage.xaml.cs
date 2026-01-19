using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Auth.Pages;

public partial class AuthSettingsPage : INavigableView<AuthViewModel>
{
    public AuthViewModel ViewModel { get; }
    public AuthSettingsPage(AuthViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}
