using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;

namespace PLCSoldier;

public partial class DeleteFileView : Window
{
    public DeleteFileView()
    {
        InitializeComponent();

        ConfirmationButton.Command = ReactiveCommand.Create(ConfirmDeletingExecute);

        CancelButton.Command = ReactiveCommand.Create(CancelDeletingExecute);
    }

    public void ConfirmDeletingExecute()
    {
        DeletingFileResultViewModel result = new DeletingFileResultViewModel() { IsDelete = true };

        Close(result);
    }

    public void CancelDeletingExecute()
    {
        DeletingFileResultViewModel result = new DeletingFileResultViewModel() { IsDelete = false };

        Close(result);
    }
}