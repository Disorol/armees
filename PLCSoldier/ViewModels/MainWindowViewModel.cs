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
        Dictionary<string, ITabItem> leftUpperItems = new Dictionary<string, ITabItem>()
        {
            {"Logical organizer", new LogicalOrganizerViewModel(){IdentificationName = "Logical organizer", Header = "Логический органайзер", Content = new LogicalOrganizerContentViewModel(){ dfsdf = new ObservableCollection<Node> { new Node(@"C:\Users\T\source\repos\ValueEditor") } }, isCloseButtonVisible = true} },
        };

        // A list containing left upper space Tabitems
        public ObservableCollection<ITabItem> LeftUpperContent { get; set; }

        public MainWindowViewModel() 
        {
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);

            LeftUpperContent = new ObservableCollection<ITabItem>();

            LeftUpperContent.Add(leftUpperItems["Logical organizer"]);
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