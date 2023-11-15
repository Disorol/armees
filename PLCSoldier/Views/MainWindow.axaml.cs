using Avalonia.Controls;
using PLCSoldier.ViewModels;

namespace PLCSoldier.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}