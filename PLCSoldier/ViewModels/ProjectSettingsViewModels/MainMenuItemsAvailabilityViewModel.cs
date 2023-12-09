using PLCSoldier.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    /*
        The view model sets the state of the isEnabled
        property for menu items responsible for opening tabs.

        The class is a singleton because it doesn't make sense to store multiple instances.
    */
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

        private bool _ValueEditor;
        public bool ValueEditor
        {
            get => _ValueEditor;
            set => this.RaiseAndSetIfChanged(ref _ValueEditor, value);
        }

        private bool _Property;
        public bool Property
        {
            get => _Property;
            set => this.RaiseAndSetIfChanged(ref _Property, value);
        }

        public MainMenuItemsAvailabilityViewModel()
        {
            LogicalOrganizer = true;
            HardwareOrganizer = true;
            Errors = true;
            SearchResults = true;
            Watch = true;
            Workspace = true;
            ValueEditor = true;
            Property = true;
        }

        /*
            This crutch is necessary because in cases where we only have the key
            to the tab, it is easier to access accessibility properties this way.
        */
        public void SetAvailabilityByKey(string key, bool isAvailable)
        { 
            switch (key)
            {
                case "Logical organizer":
                    LogicalOrganizer = isAvailable;
                    break;
                case "Hardware organizer":
                    HardwareOrganizer = isAvailable;
                    break;
                case "Errors":
                    Errors = isAvailable;
                    break;
                case "Search results":
                    SearchResults = isAvailable;
                    break;
                case "Watch":
                    Watch = isAvailable;
                    break;
                case "Workspace":
                    Workspace = isAvailable;
                    break;
                case "Value editor":
                    ValueEditor = isAvailable;
                    break;
                case "Property":
                    Property = isAvailable;
                    break;
                default: break;
            }
        }

        public bool GetAvailabilityByKey(string key)
        {
            switch (key)
            {
                case "Logical organizer":
                    return LogicalOrganizer;
                case "Hardware organizer":
                    return HardwareOrganizer;
                case "Errors":
                    return Errors;
                case "Search results":
                    return SearchResults;
                case "Watch":
                    return Watch;
                case "Workspace":
                    return Workspace;
                case "Value editor":
                    return ValueEditor;
                case "Property":
                    return Property;
                default:
                    return false;
            }
        }
    }
}
