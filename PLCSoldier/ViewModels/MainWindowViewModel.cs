﻿using Avalonia.Controls;
using DynamicData;
using PLCSoldier.Models;
using PLCSoldier.ViewModels.ProjectSettingsViewModels;
using PLCSoldier.ViewModels.TabItemViewModels;
using ReactiveUI;
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

namespace PLCSoldier.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }
        public ReactiveCommand<Unit, Unit> OpenProject { get; set; }
        public ReactiveCommand<Unit, Unit> Exit { get; set; }
        public ReactiveCommand<string, Unit> OpenTab { get; set; }
        public ReactiveCommand<string, Unit> SwitchLanguage { get; set; }

        // List of content for left upper space TabItems.
        Dictionary<string, TabItemViewModel> leftUpperItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Logical organizer", new TabItemViewModel(){IdentificationName = "Logical organizer", Header = Properties.Resources.LogicalOrganizer, isCloseButtonVisible = true, Content = new LogicalOrganizerViewModel(){ LogicalOrganizer = new ObservableCollection<Node> { new Node(@"C:\Users\T\source\repos\ValueEditor") } } } },
        };

        // List of content for left bottom space TabItems.
        Dictionary<string, TabItemViewModel> leftBottomItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Hardware Organizer", new TabItemViewModel(){IdentificationName = "Hardware Organizer", Header = Properties.Resources.HardwareOrganizer, isCloseButtonVisible = true, Content = new HardwareOrganizerViewModel() { SomeText = Properties.Resources.SomeText } }},
        };

        // List of content for central space TabItems.
        Dictionary<string, TabItemViewModel> centralItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Workspace", new TabItemViewModel(){IdentificationName = "Workspace", Header = Properties.Resources.Workspace, isCloseButtonVisible = true, Content = new WorkspaceViewModel() { SomeText = Properties.Resources.SomeText } }},
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

        // Availability of MainMenuItems
        public MainMenuItemsAvailabilityViewModel MainMenuItemsAvailability {  get; set; }

        public MainWindowViewModel() 
        {
            // The sizes of all spaces are set by default.
            SpacesDimensions = new SpacesDimensionsViewModel();

            SpacesDimensionsIntermediateСonservation = SpacesDimensionsIntermediateСonservation.getInstance();

            // All items are available by default
            MainMenuItemsAvailability = new MainMenuItemsAvailabilityViewModel();

            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);
            SwitchLanguage = ReactiveCommand.Create<string>(ExecuteSwitchLanguage);

            LeftUpperContent = new ObservableCollection<TabItemViewModel>();
            BottomContent = new ObservableCollection<TabItemViewModel>();
            LeftBottomContent = new ObservableCollection<TabItemViewModel>();
            FarRightContent = new ObservableCollection<TabItemViewModel>();
            CentralContent = new ObservableCollection<TabItemViewModel>();

            LeftUpperContent.Add(leftUpperItems["Logical organizer"]);
            BottomContent.Add(bottomItems["Errors"]);
            BottomContent.Add(bottomItems["Search results"]);
            BottomContent.Add(bottomItems["Watch"]);
            LeftBottomContent.Add(leftBottomItems["Hardware Organizer"]);
            FarRightContent.Add(farRightItems["Property"]);
            CentralContent.Add(centralItems["Workspace"]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Closing tabs by deleting them from the collection.
        private void ExecuteDeleteTabItem(string key)
        {
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

                        //LR_Splitter.IsVisible = false;
                    }
                    else
                    {
                        SpacesDimensions.LeftUpperSpaceHeight = new GridLength(0, GridUnitType.Pixel);
                        SpacesDimensions.LeftBottomSpaceHeight = new GridLength(1, GridUnitType.Star);

                        //LULB_Splitter.IsVisible = false;
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

                        //LR_Splitter.IsVisible = false;
                    }
                    else
                    {
                        SpacesDimensions.LeftUpperSpaceHeight = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.LeftBottomSpaceHeight = new GridLength(0, GridUnitType.Pixel);

                        //LULB_Splitter.IsVisible = false;
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

                        //LR_Splitter.IsVisible = false;
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

                        //CRB_Splitter.IsVisible = false;
                    }
                    else
                    {
                        SpacesDimensions.FarRightSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.CentralSpaceWidth = new GridLength(0, GridUnitType.Pixel);
                        

                        //CRR_Splitter.IsVisible = false;
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

                        //LR_Splitter.IsVisible = false;
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

                        //CRB_Splitter.IsVisible = false;
                    }
                    else
                    {
                        SpacesDimensions.CentralSpaceWidth = new GridLength(1, GridUnitType.Star);
                        SpacesDimensions.FarRightSpaceWidth = new GridLength(0, GridUnitType.Pixel);

                        //CRR_Splitter.IsVisible = false;
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

                        //CRB_Splitter.IsVisible = false;
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

                        //LR_Splitter.IsVisible = false;
                    }
                }
            }
        }

        private void ExecuteSwitchLanguage(string language)
        {
            Properties.Resources.Culture = new CultureInfo(language);
        }
    }
}