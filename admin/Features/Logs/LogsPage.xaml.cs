using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Logs;
public partial class LogsPage : INavigableView<LogsViewModel>
{
    public LogsViewModel ViewModel { get; init; }
    public LogsPage(LogsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}
