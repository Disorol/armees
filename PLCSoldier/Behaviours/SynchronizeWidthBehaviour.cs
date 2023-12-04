using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Behaviours
{
    public class SynchronizeWidthBehaviour : Behavior<Control>
    {
        public static readonly StyledProperty<TreeDataGrid?> SourceProperty =
            AvaloniaProperty.Register<Control, TreeDataGrid?>(nameof(Source));

        public TreeDataGrid? Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly StyledProperty<int> ColumnIndexProperty =
            AvaloniaProperty.Register<Control, int>(nameof(ColumnIndex));

        public int ColumnIndex
        {
            get => GetValue(ColumnIndexProperty);
            set => SetValue(ColumnIndexProperty, value);
        }

        private IColumn? requiredColumn = null;

        protected override void OnAttached()
        {
            base.OnAttached();
            Source.Loaded += OnSourceLoaded;
        }

        private void OnSourceLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                requiredColumn = Source?.Columns?[ColumnIndex];
            }
            catch { }

            if (requiredColumn == null) return;
            
            AssociatedObject!.Width = requiredColumn!.ActualWidth;
            requiredColumn.PropertyChanged += OnColumnPropertyChanged;
        }

        private void OnColumnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ActualWidth")
                return;

            AssociatedObject!.Width = requiredColumn!.ActualWidth;
        }
    }
}
