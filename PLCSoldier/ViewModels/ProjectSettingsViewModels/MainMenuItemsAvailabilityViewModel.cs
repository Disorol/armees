using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    public class MainMenuItemsAvailabilityViewModel
    {
        public bool LogicalOrganizer { get; set; }
        public bool HardwareOrganizer { get; set; }
        public bool Errors { get; set; }
        public bool SearchResults { get; set; }
        public bool Watch { get; set; }
        public bool Workspace { get; set; }
        public bool VariableEditor { get; set; }
        public bool Property { get; set; }

        public MainMenuItemsAvailabilityViewModel()
        {
            LogicalOrganizer = true;
            HardwareOrganizer = true;
            Errors = true;
            SearchResults = true;
            Watch = true;
            Workspace = true;
            VariableEditor = true;
            Property = true;
        }
    }
}
