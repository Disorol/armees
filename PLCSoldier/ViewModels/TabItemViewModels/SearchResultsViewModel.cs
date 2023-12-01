using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.TabItemViewModels
{
    public class SearchResultsViewModel : ViewModelBase
    {
        private string _SomeText;
        public string SomeText
        {
            get => _SomeText;
            set => this.RaiseAndSetIfChanged(ref _SomeText, value);
        }
    }
}
