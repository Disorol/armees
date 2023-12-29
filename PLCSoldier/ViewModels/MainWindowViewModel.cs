using Avalonia;
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
using System.Threading;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using System.Reactive.Linq;
using PLCSoldier.Classes;

namespace PLCSoldier.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /* 
            Interaction<...ViewModel, ...ResultViewModel?> for the ...View dialog box.
            
            ...ViewModel - the view model for the view.
            ...ResultViewModel - the view model for the return.
        */
        public Interaction<SwitchLanguageViewModel, SwitchingLanguageResultViewModel?> ShowSwitchLanguageDialog { get; }
        public Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> ShowDeleteFileDialog { get; }
        public Interaction<ReplaceFileViewModel, ReplacingFileResultViewModel?> ShowReplaceFileDialog { get; }
        public Interaction<FileHierarchyErrorViewModel, FileHierarchyErrorResultViewModel?> ShowFileHierarchyErrorDialog { get; }
        public Interaction<SameDirectoryErrorViewModel, SameDirectoryErrorResultViewModel?> ShowSameDirectoryErrorDialog { get; }

        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }
        public ReactiveCommand<Unit, Unit> OpenProject { get; set; }
        public ReactiveCommand<Unit, Unit> Exit { get; set; }
        public ReactiveCommand<string, Unit> OpenTab { get; set; }
        public ReactiveCommand<string, Unit> SwitchLanguage { get; set; }
        public ReactiveCommand<Unit, Unit> SetGUISettingsAsDefault { get; set; }

        public bool IsDefaultGUISettings { get; set; }

        // List of content for left upper space TabItems.
        Dictionary<string, TabItemViewModel> leftUpperItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Logical organizer", new TabItemViewModel(){IdentificationName = "Logical organizer", Header = Properties.Resources.LogicalOrganizer, isCloseButtonVisible = true, Content = null } },
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
            {"Value editor", new TabItemViewModel(){IdentificationName = "Value editor", Header = Properties.Resources.ValueEditor, isCloseButtonVisible = true, Content = new ValueEditorViewModel((IDialogService)new DialogService(new DialogManager(viewLocator: new ViewLocator(), dialogFactory: new DialogFactory().AddMessageBox()), viewModelFactory: x => Locator.Current.GetService(x)) ) } },
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
        private ObservableCollection<TabItemViewModel> _LeftUpperContent;
        public ObservableCollection<TabItemViewModel> LeftUpperContent
        {
            get => _LeftUpperContent;
            set => this.RaiseAndSetIfChanged(ref _LeftUpperContent, value);
        }

        // A list containing bottom space Tabitems.
        private ObservableCollection<TabItemViewModel> _BottomContent;
        public ObservableCollection<TabItemViewModel> BottomContent
        {
            get => _BottomContent;
            set => this.RaiseAndSetIfChanged(ref _BottomContent, value);
        }

        // A list containing left bottom space Tabitems.
        private ObservableCollection<TabItemViewModel> _LeftBottomContent;
        public ObservableCollection<TabItemViewModel> LeftBottomContent
        {
            get => _LeftBottomContent;
            set => this.RaiseAndSetIfChanged(ref _LeftBottomContent, value);
        }

        // A list containing far right space Tabitems.
        private ObservableCollection<TabItemViewModel> _FarRightContent;
        public ObservableCollection<TabItemViewModel> FarRightContent
        {
            get => _FarRightContent;
            set => this.RaiseAndSetIfChanged(ref _FarRightContent, value);
        }

        // A list containing central space Tabitems.
        private ObservableCollection<TabItemViewModel> _CentralContent;
        public ObservableCollection<TabItemViewModel> CentralContent
        {
            get => _CentralContent;
            set => this.RaiseAndSetIfChanged(ref _CentralContent, value);
        }

        // Dimensions of all spaces.
        private SpacesDimensionsViewModel _SpacesDimensions;
        public SpacesDimensionsViewModel SpacesDimensions
        {
            get => _SpacesDimensions;
            set => this.RaiseAndSetIfChanged(ref _SpacesDimensions, value);
        }

        // Intermediate conservation of the dimensions of spaces in pixels.
        private SpacesDimensionsIntermediateСonservation _SpacesDimensionsIntermediateСonservation;
        public SpacesDimensionsIntermediateСonservation SpacesDimensionsIntermediateСonservation
        {
            get => _SpacesDimensionsIntermediateСonservation;
            set => this.RaiseAndSetIfChanged(ref _SpacesDimensionsIntermediateСonservation, value);
        }

        // Visibility of spaces splitters.
        private SplittersVisibilityViewModel _SplittersVisibility;
        public SplittersVisibilityViewModel SplittersVisibility
        {
            get => _SplittersVisibility;
            set => this.RaiseAndSetIfChanged(ref _SplittersVisibility, value);
        }

        // Availability of MainMenuItems.
        private MainMenuItemsAvailabilityViewModel _MainMenuItemsAvailability;
        public MainMenuItemsAvailabilityViewModel MainMenuItemsAvailability
        {
            get => _MainMenuItemsAvailability;
            set => this.RaiseAndSetIfChanged(ref _MainMenuItemsAvailability, value);
        }

        public MainWindowViewModel()
        {
            IsDefaultGUISettings = false;

            if (!IsDefaultGUISettings)
            {
                JsonGUISettingsWorker.FileRead();

                if (JsonGUISettingsWorker.GUISettingsModel == null)
                    IsDefaultGUISettings = true;
            }

            if (IsDefaultGUISettings)
            {
                ExecuteSetGUISettingsAsDefault();
            }
            else
            {
                SpacesDimensions = GridLengthDeconverter.ConvertToSpacesDimensionsViewModel(JsonGUISettingsWorker.GUISettingsModel.SpacesDimensionsConverted);

                SpacesDimensionsIntermediateСonservation = GridLengthDeconverter.ConvertToSpacesDimensionsIntermediateСonservation(JsonGUISettingsWorker.GUISettingsModel.SpacesDimensionsIntermediateConservationConverted);

                SplittersVisibility = JsonGUISettingsWorker.GUISettingsModel.SplittersVisibility;

                MainMenuItemsAvailability = JsonGUISettingsWorker.GUISettingsModel.MainMenuItemsAvailability;

                CompleteCleanSpaces();

                List<TabItemViewModel> tabItems = new List<TabItemViewModel>();

                foreach (string key in GetAllKeys())
                {
                    if (!MainMenuItemsAvailability.GetAvailabilityByKey(key)) tabItems.Add(KeyToTabItem(key));
                }

                AddTabItems(tabItems);
            }

            ShowSwitchLanguageDialog = new Interaction<SwitchLanguageViewModel, SwitchingLanguageResultViewModel?>();
            ShowDeleteFileDialog = new Interaction<DeleteFileViewModel, DeletingFileResultViewModel?>();
            ShowReplaceFileDialog = new Interaction<ReplaceFileViewModel, ReplacingFileResultViewModel?>();
            ShowFileHierarchyErrorDialog = new Interaction<FileHierarchyErrorViewModel, FileHierarchyErrorResultViewModel?>();
            ShowSameDirectoryErrorDialog = new Interaction<SameDirectoryErrorViewModel, SameDirectoryErrorResultViewModel?>();

            // Assigning methods to commands.
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);
            SwitchLanguage = ReactiveCommand.Create<string>(ExecuteSwitchLanguage);
            OpenProject = ReactiveCommand.Create(ExecuteOpenProject);
            OpenTab = ReactiveCommand.Create<string>(ExecuteOpenTab);
            SetGUISettingsAsDefault = ReactiveCommand.Create(ExecuteSetGUISettingsAsDefault);

            JsonGUISettingsWorker.StartSaveTimer(SpacesDimensions, SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailability, SplittersVisibility);
        }

        // Adding tabs to display collections.
        private void AddTabItems(List<TabItemViewModel> tabItems)
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

        // Getting all the keys of all the tab dictionaries.
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

        // Returning tab objects by dictionary keys.
        private TabItemViewModel KeyToTabItem(string key)
        {
            if (leftUpperItems.ContainsKey(key)) return leftUpperItems[key];
            else if (leftBottomItems.ContainsKey(key)) return leftBottomItems[key];
            else if (centralItems.ContainsKey(key)) return centralItems[key];
            else if (farRightItems.ContainsKey(key)) return farRightItems[key];
            else if (bottomItems.ContainsKey(key)) return bottomItems[key];

            return null;
        }


        // Clearing spaces from tabs by assigning new links to variables.
        private void CompleteCleanSpaces()
        {
            LeftUpperContent = new ObservableCollection<TabItemViewModel>();
            BottomContent = new ObservableCollection<TabItemViewModel>();
            LeftBottomContent = new ObservableCollection<TabItemViewModel>();
            FarRightContent = new ObservableCollection<TabItemViewModel>();
            CentralContent = new ObservableCollection<TabItemViewModel>();
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
            OpenFileDialog dialog = new();
            dialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "arm" } });
            dialog.AllowMultiple = false;
            Window? mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

            string[]? result = await dialog.ShowAsync(mainWindow);

            if (result != null)
            {
                ObservableCollection<Node> nodes = ArmFileWorker.GetNodes(result[0]);
                
                leftUpperItems["Logical organizer"].Content = new LogicalOrganizerViewModel(ShowDeleteFileDialog, ShowReplaceFileDialog, ShowFileHierarchyErrorDialog, ShowSameDirectoryErrorDialog, centralItems, CentralContent) { LogicalOrganizer = nodes };
            }
        }

        private async void ExecuteSwitchLanguage(string language)
        {
            if (language != Properties.Resources.Culture.Name)
            {
                SwitchLanguageViewModel switchLanguageViewModel = new SwitchLanguageViewModel();

                SwitchingLanguageResultViewModel interactionResult = await ShowSwitchLanguageDialog.Handle(switchLanguageViewModel);

                if (interactionResult != null)
                {
                    if (interactionResult.IsReboot)
                    {
                        SaveBeforeClosing.ApplicationLanguage = null;
                        Properties.Resources.Culture = new CultureInfo(language);
                        JsonGUISettingsWorker.SaveNow();

                        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                            lifetime.Shutdown();
                    }
                    else
                    {
                        SaveBeforeClosing.ApplicationLanguage = language;
                    }
                }
            }
            else if (language != SaveBeforeClosing.ApplicationLanguage)
            {
                SaveBeforeClosing.ApplicationLanguage = null;
            }
        }

        private void ExecuteSetGUISettingsAsDefault()
        {
            JsonGUISettingsWorker.PauseSaveTimer();

            SpacesDimensions = new SpacesDimensionsViewModel();

            SpacesDimensionsIntermediateСonservation = new SpacesDimensionsIntermediateСonservation();

            SplittersVisibility = new SplittersVisibilityViewModel();

            MainMenuItemsAvailability = new MainMenuItemsAvailabilityViewModel();

            CompleteCleanSpaces();

            AddTabItems(new List<TabItemViewModel> { leftUpperItems["Logical organizer"], leftBottomItems["Hardware organizer"],
                                                                                        centralItems["Workspace"], farRightItems["Property"], bottomItems["Errors"],
                                                                                        bottomItems["Search results"], bottomItems["Watch"] });

            JsonGUISettingsWorker.StartSaveTimer(SpacesDimensions, SpacesDimensionsIntermediateСonservation, MainMenuItemsAvailability, SplittersVisibility);
        }
    }
}