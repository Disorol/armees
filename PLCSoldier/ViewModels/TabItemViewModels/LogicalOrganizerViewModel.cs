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

        // The command for the context menu item.
        public ReactiveCommand<string, Unit>? DeleteFile { get; set; }

        // The variable to which the reference to the object created in the MainWindowViewModel will be bound.
        public Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> ShowDeleteFileDialog { get; }

        public LogicalOrganizerViewModel(Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> showDeleteFileDialog)
        {
            DeleteFile = ReactiveCommand.Create<string>(ExecuteDeleteFile);

            // Binding to an object that is created in the MainWindowViewModel.
            ShowDeleteFileDialog = showDeleteFileDialog;
        }

        private async void ExecuteDeleteFile(string path)
        {
            DeleteFileViewModel deleteFileViewModel = new DeleteFileViewModel();

            DeletingFileResultViewModel interactionResult = await ShowDeleteFileDialog.Handle(deleteFileViewModel);

            if (interactionResult != null && interactionResult.IsDelete) 
            {
                // Was it possible to delete the file?
                bool isDeleted = false;

                if (Directory.Exists(path)) // Deleting a directory.
                {
                    Directory.Delete(path, true);
                    isDeleted = true;
                }
                else if (File.Exists(path)) // Deleting a file.
                {
                    File.Delete(path);
                    isDeleted = true;
                }

                if (isDeleted && LogicalOrganizer != null && LogicalOrganizer[0].PathString != null)
                    if (path != LogicalOrganizer[0].PathString)
                    {
                        // A list of file path strings whose nodes were expanded before deletion.
                        List<string> allExpandedNodes = new List<string>();

                        // Traversing through all nodes and finding all open nodes.
                        NodeWorker.FindAllExpandedNodes(LogicalOrganizer, allExpandedNodes);

                        // Creating a new file tree after deletion.
                        LogicalOrganizer = new ObservableCollection<Node>() { new Node(LogicalOrganizer[0].PathString, true, allExpandedNodes) };
                    }
                    else
                        // The root folder of the logical organizer has been deleted.
                        LogicalOrganizer = null;
            }
        }
    }
}
