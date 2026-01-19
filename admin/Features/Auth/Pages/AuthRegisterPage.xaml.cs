using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Auth.Pages;

public partial class AuthRegisterPage : INavigableView<AuthViewModel>
{
    public AuthViewModel ViewModel { get; }

    public AuthRegisterPage(AuthViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}