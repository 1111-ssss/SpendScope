using admin.ViewModels;
using System.Windows.Controls;

namespace admin.Views.Pages;

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