using Avalonia.Controls;
using Avalonia.ReactiveUI;
using PLCSoldier.ViewModels;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace PLCSoldier.Views.DialogBoxViews
{
    public partial class SwitchLanguageView : ReactiveWindow<MainWindowViewModel>
    {
        public SwitchLanguageView()
        {
            InitializeComponent();
        }

        // Close(new SwitchingLanguageResultViewModel() { IsReboot = true });
    }
}
