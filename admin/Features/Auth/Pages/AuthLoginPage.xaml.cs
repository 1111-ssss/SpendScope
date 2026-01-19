using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Auth.Pages;

public partial class AuthLoginPage : INavigableView<AuthViewModel>
{
    public AuthViewModel ViewModel { get; }

    public AuthLoginPage(AuthViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}
