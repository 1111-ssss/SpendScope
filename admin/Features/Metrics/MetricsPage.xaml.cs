using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Metrics;

public partial class MetricsPage : INavigableView<MetricsViewModel>
{
    public MetricsViewModel ViewModel { get; set; }
    public MetricsPage(MetricsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}