using HarfBuzzSharp;
using PLCSoldier.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public static class JsonGUISettingsWorker
    {
        public static GUISetttingsModel GUISetttingsModel { get; set; }

        public static async void FileWrite()
        {
            if (GUISetttingsModel != null) 
            {
                using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync<GUISetttingsModel>(fileStream, GUISetttingsModel);
                }
            }
        }

        public static async void FileRead()
        {
            using (FileStream fileStream = new FileStream("settings.json", FileMode.OpenOrCreate))
            {
                GUISetttingsModel = await JsonSerializer.DeserializeAsync<GUISetttingsModel>(fileStream);
            }
        }
    }
}
