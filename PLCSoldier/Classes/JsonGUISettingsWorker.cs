using Avalonia.Controls;
using HarfBuzzSharp;
using PLCSoldier.Models;
using PLCSoldier.ViewModels;
using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PLCSoldier.Classes
{
    public static class JsonGUISettingsWorker
    {
        public static GUISettingsModel? GUISettingsModel { get; set; }
        private static TimerCallback? TimerCallback { get; set; }
        private static Timer? SaveTimer { get; set; }
        private static bool IsTimerRun { get; set; }
        public static int SavingInterval { get; set; } = 10000;

        public static void FileWrite()
        {
            if (GUISettingsModel != null)
            {
                try
                {
                    File.WriteAllText("settings.json", JsonSerializer.Serialize(GUISettingsModel, GetSerializerSettings()));
                }
                catch (IOException) { }
            }
        }

        public static void FileRead()
        {
            try
            {
                string readText = File.ReadAllText("settings.json");

                if (!string.IsNullOrEmpty(readText))
                    GUISettingsModel = JsonSerializer.Deserialize<GUISettingsModel>(readText, GetSerializerSettings());
                else
                    GUISettingsModel = null;
            }
            catch (IOException) { }
        }

        private static JsonSerializerOptions GetSerializerSettings()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };
        }

        public static void SaveChanges(object obj)
        {
            List<object> settingsList = (List<object>)obj;

            GUISettingsModel = new GUISettingsModel
            {
                SpacesDimensionsConverted = new SpacesDimensionsConverted((SpacesDimensionsViewModel)settingsList[0]),
                SpacesDimensionsIntermediateConservationConverted = new SpacesDimensionsIntermediateConservationConverted((SpacesDimensionsIntermediateСonservation)settingsList[1]),
                MainMenuItemsAvailability = (MainMenuItemsAvailabilityViewModel)settingsList[2],
                SplittersVisibility = (SplittersVisibilityViewModel)settingsList[3],
                ApplicationLanguage = Properties.Resources.Culture.Name
            };

            FileWrite();
        }

        public static void StartSaveTimer(SpacesDimensionsViewModel SpacesDimensions, SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailabilityViewModel MainMenuItemsAvailability, SplittersVisibilityViewModel SplittersVisibility)
        {
            TimerCallback = new TimerCallback(SaveChanges);

            SaveTimer = new Timer(TimerCallback, new List<object> { SpacesDimensions, SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailability, SplittersVisibility }, 0, SavingInterval);

            IsTimerRun = true;
        }

        public static void PauseSaveTimer()
        {
            SaveTimer?.Change(Timeout.Infinite, Timeout.Infinite);

            IsTimerRun = false;
        }

        public static void ContinueSaveTimer()
        {
            SaveTimer?.Change(0, SavingInterval);

            IsTimerRun = true;
        }

        public static void SaveNow()
        {
            if (IsTimerRun)
            {
                PauseSaveTimer();
                ContinueSaveTimer();
            }
            else
            {
                ContinueSaveTimer();
                PauseSaveTimer();
            }
        }
    }
}
