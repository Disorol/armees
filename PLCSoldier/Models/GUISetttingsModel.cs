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
    public class GUISetttingsModel
    {
        public SpacesDimensionsViewModel SpacesDimensionsViewModel { get; set; }
        public SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation { get; set; }
        public MainMenuItemsAvailabilityViewModel MainMenuItemsAvailabilityViewModel { get; set; }
        public SplittersVisibilityViewModel SplittersVisibilityViewModel { get; set; }
        public CultureInfo ApplicationLanguage { get; set; }
    }
}
