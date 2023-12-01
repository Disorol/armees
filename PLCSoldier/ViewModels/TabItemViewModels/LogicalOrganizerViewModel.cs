﻿using PLCSoldier.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.TabItemViewModels
{
    /*
        The view model that defines the content for the tab of the same name. 
    */
    public class LogicalOrganizerViewModel : ViewModelBase
    {
        private ObservableCollection<Node> _LogicalOrganizer;
        public ObservableCollection<Node> LogicalOrganizer
        {
            get => _LogicalOrganizer;
            set => this.RaiseAndSetIfChanged(ref _LogicalOrganizer, value);
        }

    }
}
