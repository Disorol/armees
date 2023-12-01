using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.TabItemViewModels
{
    /*
        The view model that defines the content for the tab of the same name. 
    */
    public class WorkspaceViewModel : ViewModelBase
    {
        private string _SomeText;
        public string SomeText
        {
            get => _SomeText;
            set => this.RaiseAndSetIfChanged(ref _SomeText, value);
        }
    }
}
