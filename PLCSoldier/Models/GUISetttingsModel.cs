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
        public SpacesDimensionsViewModel SpacesDimensionsViewModel { get; private set; }
        public SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation { get; private set; }
        public MainMenuItemsAvailabilityViewModel MainMenuItemsAvailabilityViewModel { get; private set; }
        public SplittersVisibilityViewModel SplittersVisibilityViewModel { get; private set; }
        public string ApplicationLanguage { get; set; }

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
