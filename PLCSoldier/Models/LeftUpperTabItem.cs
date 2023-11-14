using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class LeftUpperTabItem : ITabItem
    {
        public string Header { get; set; }
        public ObservableCollection<Node>? TreeViewContent { get; set; }
    }
}
