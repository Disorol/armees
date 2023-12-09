﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using PLCSoldier.Models;
using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using PLCSoldier.ViewModels.TabItemViewModels;
using PLCSoldier.Views;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Text.Json;
using System.IO;

namespace PLCSoldier.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }
        public ReactiveCommand<Unit, Unit> OpenProject { get; set; }
        public ReactiveCommand<Unit, Unit> Exit { get; set; }
        public ReactiveCommand<string, Unit> OpenTab { get; set; }
        public ReactiveCommand<string, Unit> SwitchLanguage { get; set; }

        public bool isDefaultSettings { get; set; }

        // List of content for left upper space TabItems.
        Dictionary<string, TabItemViewModel> leftUpperItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Logical organizer", new TabItemViewModel(){IdentificationName = "Logical organizer", Header = Properties.Resources.LogicalOrganizer, isCloseButtonVisible = true, Content = new LogicalOrganizerViewModel(){ LogicalOrganizer = new ObservableCollection<Node> { new Node(@"C:\Users\T\source\repos\ValueEditor") } } } },
        };

        // List of content for left bottom space TabItems.
        Dictionary<string, TabItemViewModel> leftBottomItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Hardware organizer", new TabItemViewModel(){IdentificationName = "Hardware organizer", Header = Properties.Resources.HardwareOrganizer, isCloseButtonVisible = true, Content = new HardwareOrganizerViewModel() { SomeText = Properties.Resources.SomeText } }},
        };

        // List of content for central space TabItems.
        Dictionary<string, TabItemViewModel> centralItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Workspace", new TabItemViewModel(){IdentificationName = "Workspace", Header = Properties.Resources.Workspace, isCloseButtonVisible = true, Content = new WorkspaceViewModel() { SomeText = Properties.Resources.SomeText } }},
            {"Value editor", new TabItemViewModel(){IdentificationName = "Value editor", Header = Properties.Resources.ValueEditor, isCloseButtonVisible = true, Content = new ValueEditorViewModel((IDialogService)new DialogService(new DialogManager(viewLocator: new ViewLocator(), dialogFactory: new DialogFactory().AddMessageBox()), viewModelFactory: x => Locator.Current.GetService(x)) ) { } } },
        };

        // List of content for far right space TabItems.
        Dictionary<string, TabItemViewModel> farRightItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Property", new TabItemViewModel(){IdentificationName = "Property", Header = Properties.Resources.Property, isCloseButtonVisible = true, Content = new PropertyViewModel() { SomeText = Properties.Resources.SomeText } }},
        };

        // List of content for bottom space TabItems.
        Dictionary<string, TabItemViewModel> bottomItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Errors", new TabItemViewModel(){IdentificationName = "Errors", Header = Properties.Resources.Errors, isCloseButtonVisible = false, Content = new ErrorsViewModel() { SomeText = Properties.Resources.SomeText } }},
            {"Search results", new TabItemViewModel(){IdentificationName = "Search results", Header = Properties.Resources.SearchResults, isCloseButtonVisible = false, Content = new SearchResultsViewModel() { SomeText = Properties.Resources.SomeText } }},
            {"Watch", new TabItemViewModel(){IdentificationName = "Watch", Header = Properties.Resources.Watch, isCloseButtonVisible = true, Content = new WatchViewModel() { SomeText = Properties.Resources.SomeText } }},
        };

        // A list containing left upper space Tabitems.
        public ObservableCollection<TabItemViewModel> LeftUpperContent { get; set; }

        // A list containing bottom space Tabitems.
        public ObservableCollection<TabItemViewModel> BottomContent { get; set; }

        // A list containing left bottom space Tabitems.
        public ObservableCollection<TabItemViewModel> LeftBottomContent { get; set; }

        // A list containing far right space Tabitems.
        public ObservableCollection<TabItemViewModel> FarRightContent { get; set; }

        // A list containing central space Tabitems.
        public ObservableCollection<TabItemViewModel> CentralContent { get; set; }

        // Dimensions of all spaces.
        private SpacesDimensionsViewModel _SpacesDimensions;
        public SpacesDimensionsViewModel SpacesDimensions
        {
            get => _SpacesDimensions;
            set => this.RaiseAndSetIfChanged(ref _SpacesDimensions, value);
        }

        // Intermediate conservation of the dimensions of spaces in pixels.
        public SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation { get; set; }

        // Visibility of spaces splitters.
        private SplittersVisibilityViewModel _SplittersVisibility;
        public SplittersVisibilityViewModel SplittersVisibility
        {
            get => _SplittersVisibility;
            set => this.RaiseAndSetIfChanged(ref _SplittersVisibility, value);
        }

        // Availability of MainMenuItems
        public MainMenuItemsAvailabilityViewModel MainMenuItemsAvailability { get; set; }

        public MainWindowViewModel() 
        {
            isDefaultSettings = true;

            if (!isDefaultSettings) JsonGUISettingsWorker.FileRead();

            SpacesDimensions = isDefaultSettings ? new SpacesDimensionsViewModel() : JsonGUISettingsWorker.GUISetttingsModel.SpacesDimensionsViewModel;

            SpacesDimensionsIntermediateСonservation = isDefaultSettings ? new SpacesDimensionsIntermediateСonservation() : JsonGUISettingsWorker.GUISetttingsModel.SpacesDimensionsIntermediateСonservation;

            SplittersVisibility = isDefaultSettings ? new SplittersVisibilityViewModel() : JsonGUISettingsWorker.GUISetttingsModel.SplittersVisibilityViewModel;

            MainMenuItemsAvailability = isDefaultSettings ? new MainMenuItemsAvailabilityViewModel() : JsonGUISettingsWorker.GUISetttingsModel.MainMenuItemsAvailabilityViewModel;

            // Assigning methods to commands
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);
            SwitchLanguage = ReactiveCommand.Create<string>(ExecuteSwitchLanguage);
            OpenProject = ReactiveCommand.Create(ExecuteOpenProject);
            OpenTab = ReactiveCommand.Create<string>(ExecuteOpenTab);

            LeftUpperContent = new ObservableCollection<TabItemViewModel>();
            BottomContent = new ObservableCollection<TabItemViewModel>();
            LeftBottomContent = new ObservableCollection<TabItemViewModel>();
            FarRightContent = new ObservableCollection<TabItemViewModel>();
            CentralContent = new ObservableCollection<TabItemViewModel>();

            if (isDefaultSettings) AddingTabItemsAtStartup(new List<TabItemViewModel> { leftUpperItems["Logical organizer"], leftBottomItems["Hardware organizer"], 
                                                                                        centralItems["Workspace"], farRightItems["Property"], bottomItems["Errors"], 
                                                                                        bottomItems["Search results"], bottomItems["Watch"] });
            else
            {
                List<TabItemViewModel> tabItems = new List<TabItemViewModel>();

                foreach (string key in GetAllKeys())
                {
                    if (!MainMenuItemsAvailability.GetAvailabilityByKey(key)) tabItems.Add(KeyToTabItem(key));
                }

                AddingTabItemsAtStartup(tabItems);
            }


            if (isDefaultSettings)
            {
                JsonGUISettingsWorker.GUISetttingsModel = new GUISetttingsModel();
                JsonGUISettingsWorker.GUISetttingsModel.SpacesDimensionsViewModel = SpacesDimensions;
                JsonGUISettingsWorker.GUISetttingsModel.SpacesDimensionsIntermediateСonservation = SpacesDimensionsIntermediateСonservation;
                JsonGUISettingsWorker.GUISetttingsModel.SplittersVisibilityViewModel = SplittersVisibility;
                JsonGUISettingsWorker.GUISetttingsModel.MainMenuItemsAvailabilityViewModel = MainMenuItemsAvailability;
                JsonGUISettingsWorker.GUISetttingsModel.ApplicationLanguage = Properties.Resources.Culture;
                JsonGUISettingsWorker.FileWrite();
            }
        }

        private void AddingTabItemsAtStartup(List<TabItemViewModel> tabItems)
        {
            foreach (TabItemViewModel tabItem in tabItems)
            {
                MainMenuItemsAvailability.SetAvailabilityByKey(tabItem.IdentificationName, false);

                if (leftUpperItems.ContainsKey(tabItem.IdentificationName)) LeftUpperContent.Add(tabItem);
                else if (leftBottomItems.ContainsKey(tabItem.IdentificationName)) LeftBottomContent.Add(tabItem);
                else if (centralItems.ContainsKey(tabItem.IdentificationName)) CentralContent.Add(tabItem);
                else if (farRightItems.ContainsKey(tabItem.IdentificationName)) FarRightContent.Add(tabItem);
                else if (bottomItems.ContainsKey(tabItem.IdentificationName)) BottomContent.Add(tabItem);
            }
        }

        private List<string> GetAllKeys()
        {
            List<string> allKeys = new List<string>();

            foreach (string key in leftUpperItems.Keys)
            {
                allKeys.Add(key);
            }
            foreach (string key in leftBottomItems.Keys)
            {
                allKeys.Add(key);
            }
            foreach (string key in centralItems.Keys)
            {
                allKeys.Add(key);
            }
            foreach (string key in farRightItems.Keys)
            {
                allKeys.Add(key);
            }
            foreach (string key in bottomItems.Keys)
            {
                allKeys.Add(key);
            }

            return allKeys;
        }

        private TabItemViewModel KeyToTabItem(string key)
        {
            if (leftUpperItems.ContainsKey(key)) return leftUpperItems[key];
            else if (leftBottomItems.ContainsKey(key)) return leftBottomItems[key];
            else if (centralItems.ContainsKey(key)) return centralItems[key];
            else if (farRightItems.ContainsKey(key)) return farRightItems[key];
            else if (bottomItems.ContainsKey(key)) return bottomItems[key];

            return null;
        }

        // Closing tabs by deleting them from the collection.
        private void ExecuteDeleteTabItem(string key)
        {
            MainMenuItemsAvailability.SetAvailabilityByKey(key, true);

            if (leftUpperItems.ContainsKey(key))
            {
                LeftUpperContent.Remove(leftUpperItems[key]);
                
                if (LeftUpperContent.Count == 0) 
                {
                    /* 
                        Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                        It is necessary to record only the initial pixel values.
                    */
                    if (SpacesDimensions.LeftBottomSpaceHeight != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftBottomSpaceHeight != new GridLength(0, GridUnitType.Pixel))
                        SpacesDimensionsIntermediateСonservation.LeftBottomSpaceHeight = SpacesDimensions.LeftBottomSpaceHeight;

                    if (LeftBottomContent.Count == 0)
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.LeftSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.LeftSpaceWidth = SpacesDimensions.LeftSpaceWidth;

                        SpacesDimensions.LeftSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                        SpacesDimensions.RightSpaceWidth = new GridLength(1, GridUnitType.Star);

                        SplittersVisibility.LR_Splitter = false;
                    }
                    else
                    {
                        SpacesDimensions.LeftUpperSpaceHeight = new GridLength(0, GridUnitType.Pixel);
                        SpacesDimensions.LeftBottomSpaceHeight = new GridLength(1, GridUnitType.Star);

                        SplittersVisibility.LULB_Splitter = false;
                    }
                }
            }
            else if (leftBottomItems.ContainsKey(key))
            {
                LeftBottomContent.Remove(leftBottomItems[key]);

                if (LeftBottomContent.Count == 0)
                {
                    /* 
                        Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                        It is necessary to record only the initial pixel values.
                    */
                    if (SpacesDimensions.LeftBottomSpaceHeight != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftBottomSpaceHeight != new GridLength(0, GridUnitType.Pixel))
                        SpacesDimensionsIntermediateСonservation.LeftBottomSpaceHeight = SpacesDimensions.LeftBottomSpaceHeight;

                    if (LeftUpperContent.Count == 0)
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.LeftSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.LeftSpaceWidth = SpacesDimensions.LeftSpaceWidth;

                        SpacesDimensions.LeftSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                        SpacesDimensions.RightSpaceWidth = new GridLength(1, GridUnitType.Star);

                        SplittersVisibility.LR_Splitter = false;
                    }
                    else
                    {
                        SpacesDimensions.LeftUpperSpaceHeight = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.LeftBottomSpaceHeight = new GridLength(0, GridUnitType.Pixel);

                        SplittersVisibility.LULB_Splitter = false;
                    }
                }
            }
            else if (centralItems.ContainsKey(key))
            {
                CentralContent.Remove(centralItems[key]);

                if (CentralContent.Count == 0)
                {
                    /* 
                        Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                        It is necessary to record only the initial pixel values.
                    */
                    if (SpacesDimensions.FarRightSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.FarRightSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                        SpacesDimensionsIntermediateСonservation.FarRightSpaceWidth = SpacesDimensions.FarRightSpaceWidth;

                    if (BottomContent.Count == 0 && FarRightContent.Count == 0)
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.LeftSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.LeftSpaceWidth = SpacesDimensions.LeftSpaceWidth;

                        SpacesDimensions.LeftSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.RightSpaceWidth = new GridLength(0, GridUnitType.Pixel);

                        SplittersVisibility.LR_Splitter = false;
                    }
                    else if (FarRightContent.Count == 0)
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.BottomSpaceHeight != new GridLength(1, GridUnitType.Star) && SpacesDimensions.BottomSpaceHeight != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.BottomSpaceHeight = SpacesDimensions.BottomSpaceHeight;

                        SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(0, GridUnitType.Pixel);
                        SpacesDimensions.BottomSpaceHeight = new GridLength(1, GridUnitType.Star);

                        SplittersVisibility.CFRB_Splitter = false;
                    }
                    else
                    {
                        SpacesDimensions.FarRightSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.CentralSpaceWidth = new GridLength(0, GridUnitType.Pixel);


                        SplittersVisibility.CFR_Splitter = false;
                    }
                }
            }
            else if (farRightItems.ContainsKey(key))
            {
                FarRightContent.Remove(farRightItems[key]);

                if (FarRightContent.Count == 0)
                {
                    /* 
                        Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                        It is necessary to record only the initial pixel values.
                    */
                    if (SpacesDimensions.FarRightSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.FarRightSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                        SpacesDimensionsIntermediateСonservation.FarRightSpaceWidth = SpacesDimensions.FarRightSpaceWidth;

                    if (BottomContent.Count == 0 && CentralContent.Count == 0)
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.LeftSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.LeftSpaceWidth = SpacesDimensions.LeftSpaceWidth;

                        SpacesDimensions.LeftSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.RightSpaceWidth = new GridLength(0, GridUnitType.Pixel);

                        SplittersVisibility.LR_Splitter = false;
                    }
                    else if (CentralContent.Count == 0)
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.BottomSpaceHeight != new GridLength(1, GridUnitType.Star) && SpacesDimensions.BottomSpaceHeight != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.BottomSpaceHeight = SpacesDimensions.BottomSpaceHeight;

                        SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(0, GridUnitType.Pixel);
                        SpacesDimensions.BottomSpaceHeight = new GridLength(1, GridUnitType.Star);

                        SplittersVisibility.CFRB_Splitter = false;
                    }
                    else
                    {
                        SpacesDimensions.CentralSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.FarRightSpaceWidth = new GridLength(0, GridUnitType.Pixel);

                        SplittersVisibility.CFR_Splitter = false;
                    }
                }
            }
            else if (bottomItems.ContainsKey(key))
            {
                if (key == "Errors" || key == "Search results" || key == "Watch")
                {
                    BottomContent.Remove(bottomItems["Errors"]);
                    BottomContent.Remove(bottomItems["Search results"]);
                    BottomContent.Remove(bottomItems["Watch"]);

                    MainMenuItemsAvailability.Errors = true;
                    MainMenuItemsAvailability.SearchResults = true;
                    MainMenuItemsAvailability.Watch = true;
                }
                else
                {
                    BottomContent.Remove(bottomItems[key]);
                }

                if (BottomContent.Count == 0)
                {
                    /* 
                        Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                        It is necessary to record only the initial pixel values.
                    */
                    if (SpacesDimensions.BottomSpaceHeight != new GridLength(1, GridUnitType.Star) && SpacesDimensions.BottomSpaceHeight != new GridLength(0, GridUnitType.Pixel))
                        SpacesDimensionsIntermediateСonservation.BottomSpaceHeight = SpacesDimensions.BottomSpaceHeight;

                    if ((CentralContent.Count > 0) || (FarRightContent.Count > 0))
                    {
                        SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.BottomSpaceHeight = new GridLength(0, GridUnitType.Pixel);

                        SplittersVisibility.CFRB_Splitter = false;
                    }
                    else
                    {
                        /* 
                            Checking for an attempt to write values of 1 Star or 0 Pixel to TabStatus.
                            It is necessary to record only the initial pixel values.
                        */
                        if (SpacesDimensions.LeftSpaceWidth != new GridLength(1, GridUnitType.Star) && SpacesDimensions.LeftSpaceWidth != new GridLength(0, GridUnitType.Pixel))
                            SpacesDimensionsIntermediateСonservation.LeftSpaceWidth = SpacesDimensions.LeftSpaceWidth;

                        SpacesDimensions.LeftSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.RightSpaceWidth = new GridLength(0, GridUnitType.Pixel);

                        SplittersVisibility.LR_Splitter = false;
                    }
                }
            }
        }

        private void ExecuteOpenTab(string key)
        {
            if (leftUpperItems.ContainsKey(key))
            {
                LeftUpperSpaceExpansion();
                LeftSpaceExpansion();

                if (!LeftUpperContent.Contains(leftUpperItems[key]))
                {
                    LeftUpperContent.Add(leftUpperItems[key]);

                    MainMenuItemsAvailability.SetAvailabilityByKey(key, false);
                }
            }
            else if (leftBottomItems.ContainsKey(key))
            {
                LeftBottomSpaceExpansion();
                LeftSpaceExpansion();

                if (!LeftBottomContent.Contains(leftBottomItems[key]))
                {
                    LeftBottomContent.Add(leftBottomItems[key]);

                    MainMenuItemsAvailability.SetAvailabilityByKey(key, false);
                }
            }
            else if (centralItems.ContainsKey(key))
            {
                CentralSpaceExpansion();
                RightSpaceExpansion();
                CentralAndFarRightSpaceExpansion();

                if (!CentralContent.Contains(centralItems[key]))
                {
                    CentralContent.Add(centralItems[key]);

                    MainMenuItemsAvailability.SetAvailabilityByKey(key, false);
                }
            }
            else if (farRightItems.ContainsKey(key))
            {
                FarRightSpaceExpansion();
                CentralAndFarRightSpaceExpansion();
                RightSpaceExpansion();

                if (!FarRightContent.Contains(farRightItems[key]))
                {
                    FarRightContent.Add(farRightItems[key]);

                    MainMenuItemsAvailability.SetAvailabilityByKey(key, false);
                }
            }
            else if (bottomItems.ContainsKey(key))
            {
                BottomSpaceExpansion();
                RightSpaceExpansion();

                if (!BottomContent.Contains(bottomItems[key]))
                {
                    if (key == "Errors" || key == "Search results" || key == "Watch")
                    {
                        BottomContent.Add(bottomItems["Errors"]);
                        BottomContent.Add(bottomItems["Search results"]);
                        BottomContent.Add(bottomItems["Watch"]);

                        MainMenuItemsAvailability.Errors = false;
                        MainMenuItemsAvailability.SearchResults = false;
                        MainMenuItemsAvailability.Watch = false;
                    }
                    else
                    {
                        BottomContent.Add(bottomItems[key]);

                        MainMenuItemsAvailability.SetAvailabilityByKey(key, false);
                    }
                }
            }
        }

        private void LeftUpperSpaceExpansion() 
        {
            if (LeftUpperContent.Count == 0)
            {
                if (LeftBottomContent.Count == 0)
                {
                    SpacesDimensions.LeftUpperSpaceHeight = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.LeftBottomSpaceHeight = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.LeftBottomSpaceHeight = SpacesDimensionsIntermediateСonservation.LeftBottomSpaceHeight;
                    SpacesDimensions.LeftUpperSpaceHeight = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.LULB_Splitter = true;
                }
            }
        }

        private void LeftBottomSpaceExpansion()
        {
            if (LeftBottomContent.Count == 0)
            {
                if (LeftUpperContent.Count == 0)
                {
                    SpacesDimensions.LeftBottomSpaceHeight = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.LeftUpperSpaceHeight = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.LeftBottomSpaceHeight = SpacesDimensionsIntermediateСonservation.LeftBottomSpaceHeight;
                    SpacesDimensions.LeftUpperSpaceHeight = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.LULB_Splitter = true;
                }
            }
        }

        private void CentralSpaceExpansion()
        {
            if (CentralContent.Count == 0)
            {
                if (FarRightContent.Count == 0)
                {
                    SpacesDimensions.CentralSpaceWidth = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.FarRightSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.CentralSpaceWidth = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.FarRightSpaceWidth = SpacesDimensionsIntermediateСonservation.FarRightSpaceWidth;

                    SplittersVisibility.CFR_Splitter = true;
                }
            }
        }

        private void FarRightSpaceExpansion()
        {
            if (FarRightContent.Count == 0)
            {
                if (CentralContent.Count == 0)
                {
                    SpacesDimensions.FarRightSpaceWidth = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.CentralSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.FarRightSpaceWidth = SpacesDimensionsIntermediateСonservation.FarRightSpaceWidth;
                    SpacesDimensions.CentralSpaceWidth = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.CFR_Splitter = true;
                }
            }
        }

        private void BottomSpaceExpansion()
        {
            if (BottomContent.Count == 0)
            {
                if ((CentralContent.Count == 0) && (FarRightContent.Count == 0))
                {
                    SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(0, GridUnitType.Pixel);
                    SpacesDimensions.BottomSpaceHeight = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    SpacesDimensions.BottomSpaceHeight = SpacesDimensionsIntermediateСonservation.BottomSpaceHeight;
                    SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.CFRB_Splitter = true;
                }
            }
        }

        private void CentralAndFarRightSpaceExpansion() 
        {
            if (CentralContent.Count == 0 && FarRightContent.Count == 0)
            {
                if (BottomContent.Count == 0)
                {
                    SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.BottomSpaceHeight = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.BottomSpaceHeight = SpacesDimensionsIntermediateСonservation.BottomSpaceHeight;
                    SpacesDimensions.CentralAndFarRightSpacesHeight = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.CFRB_Splitter = true;
                }
            }
        }

        private void LeftSpaceExpansion() 
        {
            if (LeftUpperContent.Count == 0 && LeftBottomContent.Count == 0)
            {
                if (BottomContent.Count == 0 && CentralContent.Count == 0 && FarRightContent.Count == 0)
                {
                    SpacesDimensions.LeftSpaceWidth = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.RightSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.LeftSpaceWidth = SpacesDimensionsIntermediateСonservation.LeftSpaceWidth;
                    SpacesDimensions.RightSpaceWidth = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.LR_Splitter = true;
                }
            }
        }

        private void RightSpaceExpansion() 
        {
            if (CentralContent.Count == 0 && FarRightContent.Count == 0 && BottomContent.Count == 0)
            {
                if (LeftUpperContent.Count == 0 && LeftBottomContent.Count == 0)
                {
                    SpacesDimensions.RightSpaceWidth = new GridLength(1, GridUnitType.Star);
                    SpacesDimensions.LeftSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    SpacesDimensions.LeftSpaceWidth = SpacesDimensionsIntermediateСonservation.LeftSpaceWidth;
                    SpacesDimensions.RightSpaceWidth = new GridLength(1, GridUnitType.Star);

                    SplittersVisibility.LR_Splitter = true;
                }
            }
        }

        private async void ExecuteOpenProject()
        {
            var dialog = new OpenFolderDialog();

            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

            var result = await dialog.ShowAsync(mainWindow);

            if (result != null)
            {
                leftUpperItems["Logical organizer"].Content = new LogicalOrganizerViewModel() { LogicalOrganizer = new ObservableCollection<Node> { new Node(result) } };
            }
        }

        private void ExecuteSwitchLanguage(string language)
        {
            JsonGUISettingsWorker.GUISetttingsModel.ApplicationLanguage = new CultureInfo(language);

            JsonGUISettingsWorker.FileWrite();
        }
    }

    public class Settings
    {
        public string Language { get; set; }

        public Settings(string language)
        {
            Language = language;
        }
    } 
}