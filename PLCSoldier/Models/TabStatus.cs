using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public static class TabStatus
    {
        /*  
            Collection of the original sizes set before closing, tabs.

            Only the size values of FarRightSpace, BottomSpace, LeftBottomSpace and LeftSpace are stored,
            because the size values of neighboring spaces are set by a star.
        */
        public static Dictionary<string, GridLength> gridLengths = new Dictionary<string, GridLength>
        {
            {"FarRight_Width", new GridLength()},
            {"Bottom_Height", new GridLength()},
            {"LULB_Width", new GridLength()},
            {"LeftBottom_Height", new GridLength()},
        };
    }
}
