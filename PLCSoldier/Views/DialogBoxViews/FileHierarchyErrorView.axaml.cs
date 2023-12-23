using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;
using System;

namespace PLCSoldier;

public partial class FileHierarchyErrorView : Window
{
    public FileHierarchyErrorView()
    {
        InitializeComponent();

        SkipButton.Command = ReactiveCommand.Create(SkipExecute);;
    }

    public void SkipExecute()
    {
        Close(null);
    }
}