using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.TabItemViewModels
{
    public class WatchViewModel : ViewModelBase
    {
        private ObservableCollection<Node> _LogicalOrganizer;
        public ObservableCollection<Node> LogicalOrganizer
        {
            get => _LogicalOrganizer;
            set => this.RaiseAndSetIfChanged(ref _LogicalOrganizer, value);
        }
        public string SomeText { get; set; }
    }
}
