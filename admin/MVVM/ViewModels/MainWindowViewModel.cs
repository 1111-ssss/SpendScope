using CommunityToolkit.Mvvm.ComponentModel;
using admin.MVVM.Views;

namespace admin.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object? currentPage;

        public MainViewModel()
        {
            CurrentPage = new MainWindow(this);
        }

        public void NavigateToDashboard()
        {
            CurrentPage = new DashboardPage();
        }
    }
}