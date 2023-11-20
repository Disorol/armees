using PLCSoldier.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels
{
    public class LogicalOrganizerViewModel : ViewModelBase
    {
        public ObservableCollection<Node> LogicalOrganizer { get; set; }
    }
}
