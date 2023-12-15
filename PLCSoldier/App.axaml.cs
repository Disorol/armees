using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PLCSoldier.Classes;
using PLCSoldier.ViewModels;
using PLCSoldier.Views;
using System.Globalization;
using System.IO;

namespace PLCSoldier
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (JsonGUISettingsWorker.GUISettingsModel == null) JsonGUISettingsWorker.FileRead();

            if (JsonGUISettingsWorker.GUISettingsModel != null)
                try
                {
                    Properties.Resources.Culture = new CultureInfo(JsonGUISettingsWorker.GUISettingsModel.ApplicationLanguage);
                }
                catch (IOException)
                {
                    // Setting the Russian language to the default language quality
                    Properties.Resources.Culture = new CultureInfo("ru-RU");
                }
            else
                Properties.Resources.Culture = new CultureInfo("ru-RU");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}