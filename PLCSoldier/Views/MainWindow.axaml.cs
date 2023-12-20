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

            this.WhenActivated(action => action(ViewModel!.ShowSwitchLanguageDialog.RegisterHandler(DoShowSwitchLanguageDialogAsync)));

            this.WhenActivated(action => action(ViewModel!.ShowDeleteFileDialog.RegisterHandler(DoShowDeleteFileDialogAsync)));

            this.WhenActivated(action => action(ViewModel!.ShowReplaceFileDialog.RegisterHandler(DoShowReplaceFileDialogAsync)));
        }

        private async Task DoShowSwitchLanguageDialogAsync(InteractionContext<SwitchLanguageViewModel,
                                        SwitchingLanguageResultViewModel?> interaction)
        {
            SwitchLanguageView dialog = new()
            {
                DataContext = interaction.Input
            };

            SwitchingLanguageResultViewModel? result = await dialog.ShowDialog<SwitchingLanguageResultViewModel?>(this);
            interaction.SetOutput(result);
        }

        private async Task DoShowDeleteFileDialogAsync(InteractionContext<DeleteFileViewModel,
                                        DeletingFileResultViewModel?> interaction)
        {
            DeleteFileView dialog = new()
            {
                DataContext = interaction.Input
            };

            DeletingFileResultViewModel? result = await dialog.ShowDialog<DeletingFileResultViewModel?>(this);
            interaction.SetOutput(result);
        }

        private async Task DoShowReplaceFileDialogAsync(InteractionContext<ReplaceFileViewModel,
                                        ReplacingFileResultViewModel?> interaction)
        {
            ReplaceFileView dialog = new()
            {
                DataContext = interaction.Input
            };

            ReplacingFileResultViewModel? result = await dialog.ShowDialog<ReplacingFileResultViewModel?>(this);
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