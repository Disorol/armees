using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class GUISetttingsModel
    {
        SpacesDimensionsViewModel SpacesDimensionsViewModel { get; set; }
        SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation { get; set; }
        MainMenuItemsAvailabilityViewModel MainMenuItemsAvailabilityViewModel { get; set; }
        SplittersVisibilityViewModel SplittersVisibilityViewModel { get; set; }
        string ApplicationLanguage { get; set; }

        public GUISetttingsModel(SpacesDimensionsViewModel spacesDimensionsViewModel, SpacesDimensionsIntermediateСonservation spacesDimensionsIntermediateСonservation, MainMenuItemsAvailabilityViewModel mainMenuItemsAvailabilityViewModel, SplittersVisibilityViewModel splittersVisibilityViewModel, string applicationLanguage)
        {
            SpacesDimensionsViewModel = spacesDimensionsViewModel;
            SpacesDimensionsIntermediateСonservation = spacesDimensionsIntermediateСonservation;
            MainMenuItemsAvailabilityViewModel = mainMenuItemsAvailabilityViewModel;
            SplittersVisibilityViewModel = splittersVisibilityViewModel;
            ApplicationLanguage = applicationLanguage;
        }
    }
}
