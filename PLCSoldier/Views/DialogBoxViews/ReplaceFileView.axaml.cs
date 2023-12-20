using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;

namespace PLCSoldier;

public partial class ReplaceFileView : Window
{
    public ReplaceFileView()
    {
        InitializeComponent();

        ConfirmationButton.Command = ReactiveCommand.Create(ConfirmReplacingExecute);

        CancelButton.Command = ReactiveCommand.Create(CancelReplacingExecute);
    }
    private void ConfirmReplacingExecute()
    {
        Close(new ReplacingFileResultViewModel() { IsReplace = true });
    }

    private void CancelReplacingExecute()
    {
        Close(new ReplacingFileResultViewModel() { IsReplace = false });
    }
}