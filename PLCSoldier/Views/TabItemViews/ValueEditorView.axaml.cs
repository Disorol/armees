using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using PLCSoldier.ViewModels.TabItemViewModels;
using PLCSoldier.Views;

namespace PLCSoldier.Views.TabItemViews;

public partial class ValueEditorView : UserControl
{
     private Window? _mainWindow;

     public ValueEditorView()
     {
        InitializeComponent();
     }
 
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = desktop.MainWindow;
        }
    }

    private void OnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        _mainWindow?.FocusManager?.ClearFocus();
    }

    private void OnNewVarKeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Enter)
        {
            (DataContext as ValueEditorViewModel)!.AddNewDataValue();
        }
    }

    private void OnRemoveButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
    }
}