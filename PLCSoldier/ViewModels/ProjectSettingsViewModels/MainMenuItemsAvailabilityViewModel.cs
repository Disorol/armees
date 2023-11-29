using PLCSoldier.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    public class MainMenuItemsAvailabilityViewModel : ViewModelBase
    {
        private bool _LogicalOrganizer;
        public bool LogicalOrganizer
        {
            get => _LogicalOrganizer;
            set => this.RaiseAndSetIfChanged(ref _LogicalOrganizer, value);
        }

        private bool _HardwareOrganizer;
        public bool HardwareOrganizer
        {
            get => _HardwareOrganizer;
            set => this.RaiseAndSetIfChanged(ref _HardwareOrganizer, value);
        }

        private bool _Errors;
        public bool Errors
        {
            get => _Errors;
            set => this.RaiseAndSetIfChanged(ref _Errors, value);
        }

        private bool _SearchResults;
        public bool SearchResults
        {
            get => _SearchResults;
            set => this.RaiseAndSetIfChanged(ref _SearchResults, value);
        }

        private bool _Watch;
        public bool Watch
        {
            get => _Watch;
            set => this.RaiseAndSetIfChanged(ref _Watch, value);
        }

        private bool _Workspace;
        public bool Workspace
        {
            get => _Workspace;
            set => this.RaiseAndSetIfChanged(ref _Workspace, value);
        }

        private bool _VariableEditor;
        public bool VariableEditor
        {
            get => _VariableEditor;
            set => this.RaiseAndSetIfChanged(ref _VariableEditor, value);
        }

        private bool _Property;
        public bool Property
        {
            get => _Property;
            set => this.RaiseAndSetIfChanged(ref _Property, value);
        }

        private MainMenuItemsAvailabilityViewModel()
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

        private static MainMenuItemsAvailabilityViewModel instance;

        public static MainMenuItemsAvailabilityViewModel getInstance()
        {
            if (instance == null)
                instance = new MainMenuItemsAvailabilityViewModel();
            return instance;
        }
    }
}
