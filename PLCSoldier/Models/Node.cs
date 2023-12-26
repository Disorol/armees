using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PLCSoldier.Classes;

namespace PLCSoldier.Models
{
    public class Node
    {
        public ObservableCollection<Node>? Subnodes { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }

        [JsonIgnore]
        public bool IsExpanded { get; set; } = false;

        public Node() { }
    }
}
