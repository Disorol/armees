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
        public static GUISetttingsModel GUISetttingsModel { get; set; }

        public static void FileWrite()
        {
            if (GUISetttingsModel != null) 
            {
                using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
                {
                    JsonSerializer.Serialize<GUISetttingsModel>(fileStream, GUISetttingsModel, GetSerializerSettings());
                }
            }
        }

        public static void FileRead()
        {
            using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
            { 
                JsonSerializer.Deserialize<GUISetttingsModel>(fileStream, GetSerializerSettings());
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
