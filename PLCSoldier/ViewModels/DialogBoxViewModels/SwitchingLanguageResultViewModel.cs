using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.DialogBoxViewModels
{
    // The view model for the result of the SwitchingLanguageView view.
    public class SwitchingLanguageResultViewModel
    {
        // Will the application be restarted?
        public bool IsReboot { get; set; } = false;
    }
}
