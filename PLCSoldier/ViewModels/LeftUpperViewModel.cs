using PLCSoldier.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels
{
    public class LeftUpperViewModel : ITabItem
    {
        public string IdentificationName { get; set; }
        public string Header { get; set; }
        public bool isCloseButtonVisible { get; set; }
        public object? Content { get; set; }
    }
}
