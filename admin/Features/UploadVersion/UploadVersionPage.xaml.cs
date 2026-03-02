using Wpf.Ui.Abstractions.Controls;

namespace admin.Features.UploadVersion;

public partial class UploadVersionPage : INavigableView<UploadVersionViewModel>
{
    public UploadVersionViewModel ViewModel { get; init; }
    public UploadVersionPage(UploadVersionViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }
}
