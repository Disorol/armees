using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    /*
    The dimensions of spaces are saved here in cases
    when all tabs of one space are closed and it disappears from view.

    The sizes of only those spaces that are set in pixels are saved
    (according to the conceived logic, all neighboring spaces for the
    presented spaces are set by a star).

    Also, the "setters" take into
    account cases of attempts to record one star or zero pixels.
    Such cases may occur during the closure of the adjacent space.
    */
    public class SpacesDimensionsIntermediateСonservation
    {
        private GridLength _LeftSpaceWidth;
        public GridLength LeftSpaceWidth
        {
            get => _LeftSpaceWidth;
            set
            {
                if (value != new GridLength(1, GridUnitType.Star) && value != new GridLength(0, GridUnitType.Pixel)) _LeftSpaceWidth = value;
            }
        }

        private GridLength _LeftBottomSpaceHeight;
        public GridLength LeftBottomSpaceHeight
        {
            get => _LeftBottomSpaceHeight;
            set
            {
                if (value != new GridLength(1, GridUnitType.Star) && value != new GridLength(0, GridUnitType.Pixel)) _LeftBottomSpaceHeight = value;
            }
        }

        private GridLength _BottomSpaceHeight;
        public GridLength BottomSpaceHeight
        {
            get => _BottomSpaceHeight;
            set
            {
                if (value != new GridLength(1, GridUnitType.Star) && value != new GridLength(0, GridUnitType.Pixel)) _BottomSpaceHeight = value;
            }
        }

        private GridLength _FarRightSpaceWidth;
        public GridLength FarRightSpaceWidth
        {
            get => _FarRightSpaceWidth;
            set
            {
                if (value != new GridLength(1, GridUnitType.Star) && value != new GridLength(0, GridUnitType.Pixel)) _FarRightSpaceWidth = value;
            }
        }

        // Setting default values
        public SpacesDimensionsIntermediateСonservation() { }

        // Setting custom values
        public SpacesDimensionsIntermediateСonservation(GridLength leftSpaceWidth, GridLength leftBottomSpaceHeight, GridLength bottomSpaceHeight, GridLength farRightSpaceWidth)
        {
            LeftSpaceWidth = leftSpaceWidth;
            LeftBottomSpaceHeight = leftBottomSpaceHeight;
            BottomSpaceHeight = bottomSpaceHeight;
            FarRightSpaceWidth = farRightSpaceWidth;
        }
    }
}