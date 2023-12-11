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
        private static Timer? SaveTimer {  get; set; }

        public static void FileWrite()
        {
            if (GUISettingsModel != null) 
            {
                try
                {
                    using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
                    {
                        JsonSerializer.Serialize<GUISettingsModel>(fileStream, GUISettingsModel, GetSerializerSettings());
                    }
                }
                catch (IOException) { }
            }
        }

        public static void FileRead()
        {
            try
            {
                using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
                {
                    if (fileStream.Length > 0)
                        GUISettingsModel = JsonSerializer.Deserialize<GUISettingsModel>(fileStream, GetSerializerSettings());
                    else
                        GUISettingsModel = null;
                }
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

            GUISettingsModel = new GUISettingsModel();
            GUISettingsModel.SpacesDimensionsConverted = new SpacesDimensionsConverted((SpacesDimensionsViewModel)settingsList[0]);
            GUISettingsModel.SpacesDimensionsIntermediateConservationConverted = new SpacesDimensionsIntermediateConservationConverted((SpacesDimensionsIntermediateСonservation)settingsList[1]);
            GUISettingsModel.MainMenuItemsAvailability = (MainMenuItemsAvailabilityViewModel)settingsList[2];
            GUISettingsModel.SplittersVisibility = (SplittersVisibilityViewModel)settingsList[3];
            GUISettingsModel.ApplicationLanguage = Properties.Resources.Culture.Name;
            FileWrite();
        }

        public static void StartTimer(SpacesDimensionsViewModel SpacesDimensions, SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailabilityViewModel MainMenuItemsAvailability, SplittersVisibilityViewModel SplittersVisibility)
        {
            TimerCallback timerCallback = new TimerCallback(SaveChanges);

            SaveTimer = new Timer(timerCallback, new List<object>() { SpacesDimensions, SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailability, SplittersVisibility }, 0, 2000);
        }
    }
}
