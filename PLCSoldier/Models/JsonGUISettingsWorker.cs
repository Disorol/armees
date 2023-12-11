using Avalonia.Controls;
using HarfBuzzSharp;
using PLCSoldier.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public static class JsonGUISettingsWorker
    {
        public static GUISettingsModel? GUISettingsModel { get; set; }

        public static void FileWrite()
        {
            if (GUISettingsModel != null) 
            {
                using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
                {
                    JsonSerializer.Serialize<GUISettingsModel>(fileStream, GUISettingsModel, GetSerializerSettings());
                }
            }
        }

        public static void FileRead()
        {
            using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
            {
                if (fileStream.Length > 0)
                    GUISettingsModel = JsonSerializer.Deserialize<GUISettingsModel>(fileStream, GetSerializerSettings());
                else
                    GUISettingsModel = null;
            }
        }
        
        private static JsonSerializerOptions GetSerializerSettings()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };
        }
    }
}
