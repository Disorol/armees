using Avalonia.Controls;
using DynamicData;
using PLCSoldier.Models;
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

        // A list containing left upper space Tabitems
        public ObservableCollection<TabItemViewModel> LeftUpperContent { get; set; }

        // A list containing left upper space Tabitems
        public ObservableCollection<TabItemViewModel> BottomContent { get; set; }

        public MainWindowViewModel() 
        {
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);

            LeftUpperContent = new ObservableCollection<TabItemViewModel>();
            BottomContent = new ObservableCollection<TabItemViewModel>();

            LeftUpperContent.Add(leftUpperItems["Logical organizer"]);
            BottomContent.Add(bottomItems["Errors"]);
            BottomContent.Add(bottomItems["Search results"]);
            BottomContent.Add(bottomItems["Watch"]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }

        public void ExecuteDeleteTabItem(string obj)
        {
            if (leftUpperItems.TryGetValue(obj, out TabItemViewModel d))
            {
                LeftUpperContent.Remove(leftUpperItems["Logical organizer"]);
            }
        }
    }
}