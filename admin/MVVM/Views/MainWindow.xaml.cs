using System.Windows;
using admin.MVVM.ViewModels;

namespace admin.MVVM.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }
        public void NavigateToDashboard(object sender, RoutedEventArgs e) => _viewModel.NavigateToDashboard();
    }
}