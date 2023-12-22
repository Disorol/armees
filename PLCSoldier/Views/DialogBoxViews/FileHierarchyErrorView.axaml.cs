using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;

namespace PLCSoldier;

public partial class FileHierarchyErrorView : Window
{
    public FileHierarchyErrorView()
    {
        InitializeComponent();

        SkipButton.Command = ReactiveCommand.Create(ConfirmDeletingExecute);
    }

    public void ConfirmDeletingExecute()
    {
        Close(null);
    }
}