using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace PLCSoldier;

public partial class SameDirectoryErrorView : Window
{
    public SameDirectoryErrorView()
    {
        InitializeComponent();

        SkipButton.Command = ReactiveCommand.Create(SkipExecute);
    }

    private void SkipExecute()
    {
        Close(null);
    }
}