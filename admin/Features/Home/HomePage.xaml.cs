using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Home;

public partial class HomePage : INavigableView<HomeViewModel>
{
    public HomeViewModel ViewModel { get; }
    public HomePage(HomeViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}