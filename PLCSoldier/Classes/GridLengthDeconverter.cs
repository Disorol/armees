using Avalonia.Controls;
using PLCSoldier.Enums;
using PLCSoldier.Models;
using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    public static class GridLengthDeconverter
    {
        public static GridLength ConvertToGridLength(GridLengthConverted gridLengthConverted)
        {
            if (gridLengthConverted.typeOfValue == GridLengthType.Star)
                return new GridLength(1, GridUnitType.Star);
            else
                return new GridLength(gridLengthConverted.Value, GridUnitType.Pixel);
        }

        public static SpacesDimensionsViewModel ConvertToSpacesDimensionsViewModel(SpacesDimensionsConverted spacesDimensionsConverted)
        {
            return new SpacesDimensionsViewModel(ConvertToGridLength(spacesDimensionsConverted.LeftSpaceWidth),
                                                 ConvertToGridLength(spacesDimensionsConverted.RightSpaceWidth),
                                                 ConvertToGridLength(spacesDimensionsConverted.LeftUpperSpaceHeight),
                                                 ConvertToGridLength(spacesDimensionsConverted.LeftBottomSpaceHeight),
                                                 ConvertToGridLength(spacesDimensionsConverted.CentralAndFarRightSpacesHeight),
                                                 ConvertToGridLength(spacesDimensionsConverted.BottomSpaceHeight),
                                                 ConvertToGridLength(spacesDimensionsConverted.CentralSpaceWidth),
                                                 ConvertToGridLength(spacesDimensionsConverted.FarRightSpaceWidth));
        }

        public static SpacesDimensionsIntermediateСonservation ConvertToSpacesDimensionsIntermediateСonservation(SpacesDimensionsIntermediateConservationConverted conservationConverted)
        {
            return new SpacesDimensionsIntermediateСonservation(ConvertToGridLength(conservationConverted.LeftSpaceWidth),
                                                                ConvertToGridLength(conservationConverted.LeftBottomSpaceHeight),
                                                                ConvertToGridLength(conservationConverted.BottomSpaceHeight),
                                                                ConvertToGridLength(conservationConverted.FarRightSpaceWidth));
        }
    }
}