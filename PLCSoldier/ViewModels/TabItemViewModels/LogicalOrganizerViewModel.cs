using PLCSoldier.Classes;
using PLCSoldier.Models;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

        public Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> ShowDeleteFileDialog { get; }

        public LogicalOrganizerViewModel(Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> showDeleteFileDialog)
        {
            DeleteFile = ReactiveCommand.Create<string>(ExecuteDeleteFile);

            ShowDeleteFileDialog = showDeleteFileDialog;
        }

        private async void ExecuteDeleteFile(string path)
        {
            DeleteFileViewModel deleteFileViewModel = new DeleteFileViewModel();

            DeletingFileResultViewModel interactionResult = await ShowDeleteFileDialog.Handle(deleteFileViewModel);


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
                    List<string> allExpandedNodes = new List<string>();
                    NodeWorker.FindAllExpandedNodes(LogicalOrganizer, allExpandedNodes);
                    LogicalOrganizer = new ObservableCollection<Node>() { new Node(LogicalOrganizer[0].PathString, true, allExpandedNodes) };
                }
                else
                    LogicalOrganizer = null;
        }
    }
}
