using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.TabItemViewModels
{
    // Shared ViewModel for all tabs.
    public class TabItemViewModel : ViewModelBase
    {
        public string IdentificationName { get; set; }  // The key for the dictionary in which the tab model will be located.
        public string Header { get; set; }  // Tab title.
        public bool isCloseButtonVisible { get; set; }  // Visibility of the tab close button.

        /* 
            Contents of the tab. 
            A ViewModel with the property as content will be placed here. 
            Such ViewModels should be stored in the directory with this model.
        */

        private object? _Content;
        public object? Content
        {
            get => _Content; 
            set => this.RaiseAndSetIfChanged(ref _Content, value);
        }
    }
}
