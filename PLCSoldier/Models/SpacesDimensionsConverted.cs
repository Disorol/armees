using Avalonia.Controls;
using Newtonsoft.Json;
using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class SpacesDimensionsConverted
    {
        public GridLengthConverted LeftSpaceWidth { get; set; }
        public GridLengthConverted RightSpaceWidth { get; set; }
        public GridLengthConverted LeftUpperSpaceHeight { get; set; }
        public GridLengthConverted LeftBottomSpaceHeight { get; set; }
        public GridLengthConverted CentralAndFarRightSpacesHeight { get; set; }
        public GridLengthConverted BottomSpaceHeight { get; set; }
        public GridLengthConverted CentralSpaceWidth { get; set; }
        public GridLengthConverted FarRightSpaceWidth { get; set; }

        public SpacesDimensionsConverted() { }

        public SpacesDimensionsConverted(SpacesDimensionsViewModel spacesDimensions)
        {
            LeftSpaceWidth = new GridLengthConverted(spacesDimensions.LeftSpaceWidth);
            RightSpaceWidth = new GridLengthConverted(spacesDimensions.RightSpaceWidth);
            LeftUpperSpaceHeight = new GridLengthConverted(spacesDimensions.LeftUpperSpaceHeight);
            LeftBottomSpaceHeight = new GridLengthConverted(spacesDimensions.LeftBottomSpaceHeight);
            CentralAndFarRightSpacesHeight = new GridLengthConverted(spacesDimensions.CentralAndFarRightSpacesHeight);
            BottomSpaceHeight = new GridLengthConverted(spacesDimensions.BottomSpaceHeight);
            CentralSpaceWidth = new GridLengthConverted(spacesDimensions.CentralSpaceWidth);
            FarRightSpaceWidth = new GridLengthConverted(spacesDimensions.FarRightSpaceWidth);
        }
    }
}
