using System.Windows.Controls;

namespace admin.Features.Auth;

public partial class AuthView : UserControl
{
    public AuthView(AuthViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
    }
}
