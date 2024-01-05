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
        public static ObservableCollection<Node> GetNodes(string path)
        {
            string readText = File.ReadAllText(path);

            return JsonSerializer.Deserialize<ObservableCollection<Node>>(readText, GetSerializerSettings());
        }

        public static void WriteNodes(string path, ObservableCollection<Node> nodes)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(nodes, GetSerializerSettings()));
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
