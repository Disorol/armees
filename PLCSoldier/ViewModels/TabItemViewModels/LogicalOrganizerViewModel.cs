using Avalonia.Controls;
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

        // The commands for the context menu item.
        public ReactiveCommand<string, Unit>? DeleteFile { get; set; }
        public ReactiveCommand<string, Unit>? CopyFile { get; set; }
        public ReactiveCommand<string, Unit>? PasteFile { get; set; }
        public ReactiveCommand<string, Unit>? CutFile { get; set; }

        // The variable to which the reference to the object created in the MainWindowViewModel will be bound.
        public Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> ShowDeleteFileDialog { get; }

        // If the file has not been copied, the paste button should be disabled
        private bool _PasteButton_IsEnabled;
        public bool PasteButton_IsEnabled
        {
            get => _PasteButton_IsEnabled;
            set => this.RaiseAndSetIfChanged(ref _PasteButton_IsEnabled, value);
        }

        // The path to the copied file or directory.
        public string? CopiedPath { get; set; }

        public LogicalOrganizerViewModel(Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> showDeleteFileDialog)
        {
            DeleteFile = ReactiveCommand.Create<string>(ExecuteDeleteFile);
            CopyFile = ReactiveCommand.Create<string>(ExecuteCopyFile);
            PasteFile = ReactiveCommand.Create<string>(ExecutePasteFile);
            CutFile = ReactiveCommand.Create<string>(ExecuteCutFile);

            // Binding to an object that is created in the MainWindowViewModel.
            ShowDeleteFileDialog = showDeleteFileDialog;

            // The paste button must be disabled before the first copy.
            PasteButton_IsEnabled = false;
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

        private async void ExecuteCopyFile(string copyPath)
        {
            CopiedPath = copyPath;

            if (!PasteButton_IsEnabled)
                PasteButton_IsEnabled = true;
        }

        private async void ExecutePasteFile(string pastePath)
        {
            if (CopiedPath == null)
                return;

            FileInfo copiedPathInfo = new FileInfo(CopiedPath);

            FileInfo pastePathInfo = new FileInfo(pastePath);

            if (File.GetAttributes(pastePath) == FileAttributes.Directory) // This is the directory.
            {
                // If a directory is selected, the name of the copied file is assigned to it.
                pastePath += "\\" + copiedPathInfo.Name;
            }
            else // This is a file.
            {
                // If a file is selected in the directory, the name of the current file is replaced by the name of the one being copied.
                // The copied file is saved to the parent folder of the selected file.
                pastePath = pastePathInfo.DirectoryName + "\\" + copiedPathInfo.Name;
            }

            if (File.GetAttributes(CopiedPath) == FileAttributes.Directory) // This is the directory.
            {

            }
            else // This is a file.
            {
                if (!copiedPathInfo.Exists)
                {
                    File.Copy(CopiedPath, pastePath);
                }
                else
                {

                }
            }      
        }

        private async void ExecuteCutFile(string path)
        {
            return;
        }
    }
}
