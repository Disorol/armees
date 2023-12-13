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

            ConfirmationButton.Command = ReactiveCommand.Create(ConfirmRebootingExecute);

            CancelButton.Command = ReactiveCommand.Create(CancelRebootingExecute);
        }

        private void ConfirmRebootingExecute()
        {
            Close(new SwitchingLanguageResultViewModel() { IsReboot = true });
        }

        private void CancelRebootingExecute()
        {
            Close(new SwitchingLanguageResultViewModel() { IsReboot = false });
        }
    }
}
