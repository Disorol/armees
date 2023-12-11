using Avalonia.Controls;
using HarfBuzzSharp;
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

namespace PLCSoldier.Models
{
    public static class JsonGUISettingsWorker
    {
        public static GUISettingsModel? GUISettingsModel { get; set; }
        private static TimerCallback? TimerCallback { get; set; }
        private static Timer? SaveTimer {  get; set; }

        public static void FileWrite()
        {
            if (GUISettingsModel != null) 
            {
                try
                {
                    using FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate);
                        JsonSerializer.Serialize<GUISettingsModel>(fileStream, GUISettingsModel, GetSerializerSettings());
                }
                catch (IOException) { }
            }
        }

        public static void FileRead()
        {
            try
            {
                using FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate);
                    if (fileStream.Length > 0)
                        GUISettingsModel = JsonSerializer.Deserialize<GUISettingsModel>(fileStream, GetSerializerSettings());
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

            SaveTimer = new Timer(TimerCallback, new List<object> { SpacesDimensions, SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailability, SplittersVisibility }, 0, 2000);
        }

        public static void PauseSaveTimer()
        {
            SaveTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public static void ContinueSaveTimer()
        {
            SaveTimer?.Change(0, 2000);
        }
    }
}
