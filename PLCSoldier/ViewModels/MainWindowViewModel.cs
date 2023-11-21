using Avalonia.Controls;
using DynamicData;
using PLCSoldier.Models;
using PLCSoldier.ViewModels.TabItemViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PLCSoldier.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // List of content for left upper space TabItems
        Dictionary<string, TabItemViewModel> leftUpperItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Logical organizer", new TabItemViewModel(){IdentificationName = "Logical organizer", Header = "Логический органайзер", isCloseButtonVisible = true, Content = new LogicalOrganizerViewModel(){ LogicalOrganizer = new ObservableCollection<Node> { new Node(@"C:\Users\T\source\repos\ValueEditor") } } } },
        };

        // List of content for bottom space TabItems
        Dictionary<string, TabItemViewModel> bottomItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Errors", new TabItemViewModel(){IdentificationName = "Errors", Header = "Ошибки", isCloseButtonVisible = false, Content = new ErrorsViewModel() { SomeText = "Some text" } }},
            {"Search results", new TabItemViewModel(){IdentificationName = "Search results", Header = "Поиск результатов", isCloseButtonVisible = false, Content = new SearchResultsViewModel() { SomeText = "Some text" } }},
            {"Watch", new TabItemViewModel(){IdentificationName = "Watch", Header = "Просмотр", isCloseButtonVisible = true, Content = new WatchViewModel() { SomeText = "Some text" } }},
        };

        // List of content for left bottom space TabItems
        Dictionary<string, TabItemViewModel> leftBottomItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Hardware Organizer", new TabItemViewModel(){IdentificationName = "Hardware Organizer", Header = "Аппаратный органайзер", isCloseButtonVisible = true, Content = new HardwareOrganizerViewModel() { SomeText = "Some text" } }},
        };

        // List of content for far right space TabItems
        Dictionary<string, TabItemViewModel> farRightItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Property", new TabItemViewModel(){IdentificationName = "Property", Header = "Свойства", isCloseButtonVisible = true, Content = new PropertyViewModel() { SomeText = "Some text" } }},
        };

        // List of content for central space TabItems
        Dictionary<string, TabItemViewModel> centralItems = new Dictionary<string, TabItemViewModel>()
        {
            {"Workspace", new TabItemViewModel(){IdentificationName = "Workspace", Header = "Рабочая область", isCloseButtonVisible = true, Content = new WorkspaceViewModel() { SomeText = "Some text" } }},
        };

        // A list containing left upper space Tabitems
        public ObservableCollection<TabItemViewModel> LeftUpperContent { get; set; }

        // A list containing bottom space Tabitems
        public ObservableCollection<TabItemViewModel> BottomContent { get; set; }

        // A list containing left bottom space Tabitems
        public ObservableCollection<TabItemViewModel> LeftBottomContent { get; set; }

        // A list containing far right space Tabitems
        public ObservableCollection<TabItemViewModel> FarRightContent { get; set; }

        // A list containing central space Tabitems
        public ObservableCollection<TabItemViewModel> CentralContent { get; set; }

        public SpacesDimensions SpacesDimensions { get; set; }

        public MainWindowViewModel() 
        {
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);

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

            SpacesDimensions = new SpacesDimensions()
            {
                LeftSpaceWidth = new GridLength(300, GridUnitType.Pixel),
                RightSpaceWidth = new GridLength(1, GridUnitType.Star),
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }

        public void ExecuteDeleteTabItem(string key)
        {
            if (leftUpperItems.TryGetValue(key, out TabItemViewModel d))
            {
                LeftUpperContent.Remove(leftUpperItems["Logical organizer"]);
                
            }
        }
    }
}