using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace PLCSoldier.Classes
{
    /*
        A static class for assigning icons to files based on their extensions.
    */
    public static class ExtensionToIcon
    {
        // Dictionary of extensions and paths to icons.
        private static Dictionary<string, Avalonia.Media.Imaging.Bitmap> Icons = new Dictionary<string, Avalonia.Media.Imaging.Bitmap>()
        {
            {".txt", new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(new Uri("avares://PLCSoldier/Assets/images/icons/txt.png")))},
            {".dock", new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(new Uri("avares://PLCSoldier/Assets/images/icons/dock.png")))},
            {"unknown file", new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(new Uri("avares://PLCSoldier/Assets/images/icons/unknown-file.png")))},
        };

        // Getting an icon by path.
        public static Avalonia.Media.Imaging.Bitmap GetIcon(string extension)
        {
            if (Icons.TryGetValue(extension, out var icon))
                return icon;
            else
                return Icons["unknown file"];
        }
    }
}
