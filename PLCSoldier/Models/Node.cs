using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
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
        public ObservableCollection<MenuItem>? ContextMenu { get; set; }

        public Node() 
        {
            ContextMenu = new ObservableCollection<MenuItem> { new MenuItem { Header = "Copy", Name = "Copy" }, new MenuItem { Header = "Paste", Name = "Paste" }, new MenuItem { Header = "Delete", Name = "Delete" } };
        }

        // Command assignment method for all Nodes
        // The Name property of the MenuItem object of the ContextMenu collection is the key to the corresponding command.
        public static void SetCommands(ObservableCollection<Node> nodes, Dictionary<string, LogicalOrganizerCommand> commands)
        {
            // Iterating through all Nodes individually.
            foreach (var node in nodes) 
            {
                // Checking for the existence of the context menu and its items.
                if (node.ContextMenu != null && node.ContextMenu.Count > 0)
                {
                    // Iterating through all the context menu items.
                    foreach (var menuItem in node.ContextMenu)
                    {
                        // Checking for the existence of the name of the menu item and the existence of such a command in the dictionary.
                        if (menuItem.Name != null && commands.ContainsKey(menuItem.Name))
                        {
                            // Command assignment.
                            menuItem.Command = commands[menuItem.Name].Command;

                            // Checking the existence of an input parameter.
                            if (commands[menuItem.Name].CommandParameter != null)
                            {
                                // Assignment of an input parameter.
                                menuItem.CommandParameter = node.Path;
                            }
                        }
                    }
                }

                // Checking the existence of child Nodes.
                if (node.Subnodes != null && node.Subnodes.Count > 0)
                {
                    // Their processing.
                    SetCommands(node.Subnodes, commands);
                }
            }
        }

        // Finding and deleting a Node by path.
        public static void DeleteNodeByPath(ObservableCollection<Node> nodes, string deletePath)
        {
            foreach (var node in nodes)
            {
                if (node.Path != deletePath)
                {
                    if (node.Subnodes != null && node.Subnodes.Count > 0)
                    {
                        DeleteNodeByPath(node.Subnodes, deletePath);
                    }
                }
                else
                {
                    nodes.Remove(node);

                    if (nodes.Count == 0)
                    {
                        nodes = null;
                    }

                    return;
                }
            }
        }
    }
}
