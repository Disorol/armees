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
        Dictionary<string, LeftUpperTabItem> leftUpperItems = new Dictionary<string, LeftUpperTabItem>()
        {
            {"Logical organizer", new LeftUpperTabItem(){IdentificationName = "Logical organizer", Header = "Логический органайзер", TreeViewContent = null} },
        };

        // A list containing left upper space Tabitems
        public ObservableCollection<LeftUpperTabItem> LeftUpperContent { get; set; }

        public MainWindowViewModel() 
        {
            DeleteTabItem = ReactiveCommand.Create<string>(ExecuteDeleteTabItem);

            LeftUpperContent = new ObservableCollection<LeftUpperTabItem>();

            LeftUpperContent.Add(leftUpperItems["Logical organizer"]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveCommand<string, Unit> DeleteTabItem { get; set; }

        public void ExecuteDeleteTabItem(string obj)
        {
            if (leftUpperItems.TryGetValue(obj, out LeftUpperTabItem d))
            {
                LeftUpperContent.Remove(leftUpperItems["Logical organizer"]);
            }
        }
    }
}