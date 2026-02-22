using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.Versions;

public partial class VersionsPage : INavigableView<VersionsViewModel>
{
    public VersionsViewModel ViewModel { get; }
    public VersionsPage(VersionsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}
