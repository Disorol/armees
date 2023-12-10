using Avalonia.Controls;
using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class GUISettingsModel
    {
        public SpacesDimensionsConverted SpacesDimensionsConverted { get; set; }
        public SpacesDimensionsIntermediateConservationConverted SpacesDimensionsIntermediateConservationConverted { get; set; }
        public MainMenuItemsAvailabilityViewModel MainMenuItemsAvailability { get; set; }
        public SplittersVisibilityViewModel SplittersVisibility { get; set; }
        public string ApplicationLanguage { get; set; }
    }
}
