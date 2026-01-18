using System.Windows.Controls;

namespace admin.Features.Home;

public partial class HomeView : Page
{
    public HomeViewModel ViewModel { get; }
    public HomeView(HomeViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}