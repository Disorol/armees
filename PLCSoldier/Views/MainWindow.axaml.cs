using Avalonia.Controls;
using Avalonia.ReactiveUI;
using PLCSoldier.Classes;
using PLCSoldier.Models;
using PLCSoldier.ViewModels;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using PLCSoldier.Views.DialogBoxViews;
using ReactiveUI;
using System.Globalization;
using System.Threading.Tasks;

namespace PLCSoldier.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();

            this.WhenActivated(action => action(ViewModel!.ShowSwitchLanguageDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<SwitchLanguageViewModel,
                                        SwitchingLanguageResultViewModel?> interaction)
        {
            SwitchLanguageView dialog = new()
            {
                DataContext = interaction.Input
            };

            SwitchingLanguageResultViewModel? result = await dialog.ShowDialog<SwitchingLanguageResultViewModel?>(this);
            interaction.SetOutput(result);
        }

        override protected void OnClosing(WindowClosingEventArgs e)
        {
            if (SaveBeforeClosing.ApplicationLanguage != null)
                Properties.Resources.Culture = new CultureInfo(SaveBeforeClosing.ApplicationLanguage);

            JsonGUISettingsWorker.SaveNow();
        }
    }
}