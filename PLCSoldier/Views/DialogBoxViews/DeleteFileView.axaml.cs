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

        YesButton.Command = ReactiveCommand.Create(YesExecute);
    }

    public void YesExecute()
    {
        DeletingFileResultViewModel result = new DeletingFileResultViewModel() { IsDelete = true };

        Close(result);
    }
}