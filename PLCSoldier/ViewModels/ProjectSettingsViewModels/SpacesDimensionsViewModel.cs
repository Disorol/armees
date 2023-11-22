using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    public class SpacesDimensionsViewModel
    {
        /*  
            Only the size values of FarRightSpace, BottomSpace, LeftBottomSpace and LeftSpace are stored,
            because the size values of neighboring spaces are set by a star.
        */

        public GridLength LeftSpaceWidth { get; set; }
        public GridLength LeftBottomSpaceHeight { get; set; }
        public GridLength BottomSpaceHeight { get; set; }
        public GridLength FarRightSpaceWidth { get; set; }

        public SpacesDimensionsViewModel() // Setting default values
        {
            LeftSpaceWidth = new GridLength(300, GridUnitType.Pixel);

            LeftBottomSpaceHeight = new GridLength(220, GridUnitType.Pixel);

            BottomSpaceHeight = new GridLength(150, GridUnitType.Pixel);

            FarRightSpaceWidth = new GridLength(150, GridUnitType.Pixel);
        }

        public SpacesDimensionsViewModel(GridLength leftSpaceWidth, GridLength leftBottomSpaceHeight, GridLength bottomSpaceHeight, GridLength farRightSpaceWidth) // Setting custom value
        {
            LeftSpaceWidth = leftSpaceWidth;
            LeftBottomSpaceHeight = leftBottomSpaceHeight;
            BottomSpaceHeight = bottomSpaceHeight;
            FarRightSpaceWidth = farRightSpaceWidth;
        }
    }
}
