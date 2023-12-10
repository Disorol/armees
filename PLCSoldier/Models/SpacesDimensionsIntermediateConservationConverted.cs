using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class SpacesDimensionsIntermediateConservationConverted
    {
        public GridLengthConverted LeftSpaceWidth { get; set; }
        public GridLengthConverted LeftBottomSpaceHeight { get; set; }
        public GridLengthConverted BottomSpaceHeight { get; set; }
        public GridLengthConverted FarRightSpaceWidth { get; set; }

        public SpacesDimensionsIntermediateConservationConverted() { }
        public SpacesDimensionsIntermediateConservationConverted(SpacesDimensionsIntermediateСonservation spacesDimensionsIntermediateСonservation)
        {
            LeftSpaceWidth = new GridLengthConverted(spacesDimensionsIntermediateСonservation.LeftSpaceWidth);
            LeftBottomSpaceHeight = new GridLengthConverted(spacesDimensionsIntermediateСonservation.LeftBottomSpaceHeight);
            BottomSpaceHeight = new GridLengthConverted(spacesDimensionsIntermediateСonservation.BottomSpaceHeight);
            FarRightSpaceWidth = new GridLengthConverted(spacesDimensionsIntermediateСonservation.FarRightSpaceWidth);
        }
    }
}
