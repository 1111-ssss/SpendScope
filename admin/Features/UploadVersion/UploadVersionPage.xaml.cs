using System.Windows;
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

    public async void Drop_UploadVersion(object sender, DragEventArgs e)
    {
        await ViewModel.DragDrop_UploadVersion(sender, e);
    }
    public void Drop_DragOver(object sender, DragEventArgs e)
    {
        ViewModel.DragDrop_DragOver(sender, e);
    }
}
