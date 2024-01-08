using PLCSoldier.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    public static class ArmFileWorker
    {
        public static string? ArmFilePath { get; set; }

        public static ObservableCollection<Node>? GetNodes()
        {
            if (ArmFilePath == null) return null;

            string readText = File.ReadAllText(ArmFilePath);

            return JsonSerializer.Deserialize<ObservableCollection<Node>>(readText, GetSerializerSettings());
        }

        public static void WriteNodes(ObservableCollection<Node> nodes)
        {
            if (ArmFilePath == null) return;

            File.WriteAllText(ArmFilePath, JsonSerializer.Serialize(nodes, GetSerializerSettings()));
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
