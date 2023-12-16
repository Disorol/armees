using PLCSoldier.Classes;
using PLCSoldier.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.TabItemViewModels
{
    /*
        The view model that defines the content for the tab of the same name. 
    */
    public class LogicalOrganizerViewModel : ViewModelBase
    {
        private ObservableCollection<Node>? _LogicalOrganizer;
        public ObservableCollection<Node>? LogicalOrganizer
        {
            get => _LogicalOrganizer;
            set => this.RaiseAndSetIfChanged(ref _LogicalOrganizer, value);
        }
        public ReactiveCommand<string, Unit>? DeleteFile { get; set; } 

        public LogicalOrganizerViewModel()
        {
            DeleteFile = ReactiveCommand.Create<string>(ExecuteDeleteFile);
        }

        private void ExecuteDeleteFile(string path)
        {
            bool isDeleted = false;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                isDeleted = true;
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
                isDeleted = true;
            }

            if (isDeleted && LogicalOrganizer != null && LogicalOrganizer[0].PathString != null)
                if (path != LogicalOrganizer[0].PathString)
                {
                    List<string> allAncestors = FileWorker.FindAllAncestorFiles(path, LogicalOrganizer[0].PathString);
                    LogicalOrganizer = new ObservableCollection<Node>() { new Node(LogicalOrganizer[0].PathString, true) };
                }
                else
                    LogicalOrganizer = null;
        }
    }
}
