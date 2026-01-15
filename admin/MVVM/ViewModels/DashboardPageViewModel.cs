using CommunityToolkit.Mvvm.ComponentModel;
using admin.MVVM.Views;

namespace admin.MVVM.ViewModels
{
    public partial class DashboardPageViewModel: ObservableObject
    {
        [ObservableProperty]
        private object? currentPage;

        public DashboardPageViewModel()
        {
            CurrentPage = new DashboardPage();
        }

        public void NavigateToDashboard()
        {
            CurrentPage = new DashboardPage();
        }
    }
}