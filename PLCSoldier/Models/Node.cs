﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PLCSoldier.Classes;

namespace PLCSoldier.Models
{
    /*
        A class with a recursive overloaded constructor for traversing through all directories
        and creating a hierarchy of directories and files using ObservableCollection.
        !!! Files and directories are considered different concepts !!!
    */
    public class Node
    {
        // Collection of child files for this directory. Can be Null for storing files.
        public ObservableCollection<Node>? Subnodes { get; set; }

        // File or directory title. Maybe Null so that empty folders can be opened.
        public string? NodeTitle { get; set; }

        // The path to this file. Maybe Null so that empty folders can be opened.
        public string? PathString { get; set; }

        // The path to the icon. Maybe Null for empty folders.
        public Bitmap? Icon { get; set; }

        // Is the TreeView branch open?
        public bool IsExpanded { get; set; } = false;

        // Overloaded Constructor for the directory path.
        public Node(string? path, bool isDirectory, List<string>? openedDirectories = null)
        {
            if (isDirectory && path != null) // This is the directory
            {
                PathString = path;

                NodeTitle = Path.GetFileName(path);

                Icon = ExtensionToIcon.GetIcon(".dock");

                Subnodes = new ObservableCollection<Node>();

                if (openedDirectories != null && openedDirectories.Contains(path) )
                    IsExpanded = true;

                if (Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    if (Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        foreach (string subpath in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
                        {
                            Node node = new Node(subpath, true, openedDirectories);

                            Subnodes.Add(node);
                        }
                    }

                    if (Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        foreach (string subpath in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
                        {
                            Node node = new Node(subpath, false, openedDirectories);

                            Subnodes.Add(node);
                        }
                    }
                }
                else
                {
                    // Creating an empty directory

                    /*
                    Node node = new Node(null, false);
                    +
                    Subnodes.Add(node);
                    */
                }
                
            }
            else if (path != null) // This is the file
            {
                PathString = path;

                NodeTitle = Path.GetFileName(path);

                FileInfo fileInfo = new FileInfo(path);

                Icon = ExtensionToIcon.GetIcon(fileInfo.Extension);
            }
            else // This is the void
            {
                NodeTitle = "[Пустота]";
            }
        } 
    }
}
