using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using PLCSoldier.Classes;
using ReactiveUI;

namespace PLCSoldier.Models
{
    public class Node
    {
        public ObservableCollection<Node>? Subnodes { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }

        [JsonIgnore]
        public Avalonia.Media.Imaging.Bitmap? Icon { get; set; }

        [JsonIgnore]
        public bool IsExpanded { get; set; } = false;

        [JsonIgnore]
        public ObservableCollection<MenuItem> ContextMenu { get; set; }

        public Node() 
        {
            ContextMenu = new ObservableCollection<MenuItem> { new MenuItem { Header = "Copy" }, new MenuItem { Header = "Paste" } };
        }
    }
}
