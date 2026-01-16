using admin.ViewModels;
using System.Windows;
using Wpf.Ui;

namespace admin.Views
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }
        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            //Appearance.SystemThemeWatcher.Watch(this);

            InitializeComponent();

            //navigationService.SetNavigationControl(RootNavigation);
        }
    }
}