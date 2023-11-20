using DynamicData;
using PLCSoldier.Interfaces;
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
        Dictionary<string, ITabItem> leftUpperItems = new Dictionary<string, ITabItem>()
        {
            {"Logical organizer", new LeftUpperViewModel(){IdentificationName = "Logical organizer", Header = "Логический органайзер", isCloseButtonVisible = true, Content = new LogicalOrganizerViewModel(){ LogicalOrganizer = new ObservableCollection<Node> { new Node(@"C:\Users\T\source\repos\ValueEditor") } } } },
        };

        // List of content for bottom space TabItems
        Dictionary<string, ITabItem> bottomItems = new Dictionary<string, ITabItem>()
        {
            {"Errors", new BottomViewModel(){IdentificationName = "Errors", Header = "Ошибки", isCloseButtonVisible = false, Content = new ErrorsViewModel() { SomeText = "Some text" } }},
            {"Search results", new BottomViewModel(){IdentificationName = "Search results", Header = "Поиск результатов", isCloseButtonVisible = false }},
            {"Watch", new BottomViewModel(){IdentificationName = "Watch", Header = "Просмотр", isCloseButtonVisible = true }},
        };

        // A list containing left upper space Tabitems
        public ObservableCollection<ITabItem> LeftUpperContent { get; set; }

        // A list containing left upper space Tabitems
        public ObservableCollection<ITabItem> BottomContent { get; set; }

        public MainWindowViewModel() 
        {
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);

            LeftUpperContent = new ObservableCollection<ITabItem>();
            BottomContent = new ObservableCollection<ITabItem>();

            LeftUpperContent.Add(leftUpperItems["Logical organizer"]);
            BottomContent.Add(bottomItems["Errors"]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }

        public void ExecuteDeleteTabItem(string obj)
        {
            if (leftUpperItems.TryGetValue(obj, out ITabItem d))
            {
                LeftUpperContent.Remove(leftUpperItems["Logical organizer"]);
            }
        }
    }
}