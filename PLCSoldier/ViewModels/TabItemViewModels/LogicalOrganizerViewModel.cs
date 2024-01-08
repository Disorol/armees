using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.VisualBasic.FileIO;
using PLCSoldier.Classes;
using PLCSoldier.Models;
using PLCSoldier.ViewModels.DialogBoxViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

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

        //private Dictionary<string, TabItemViewModel> CentralItems;
        //private ObservableCollection<TabItemViewModel> CentralContent;

        // The commands for the context menu item.
        public ReactiveCommand<string, Unit>? TryDeleteFile { get; set; }
        public ReactiveCommand<string, Unit>? CopyFile { get; set; }
        public ReactiveCommand<string, Unit>? PasteFile { get; set; }
        public ReactiveCommand<string, Unit>? CutFile { get; set; }
        public ReactiveCommand<string, Unit>? CopyPath { get; set; }

        // Variables to which references to objects created in the MainWindowViewModel will be bound.
        public Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> ShowDeleteFileDialog { get; }
        public Interaction<ReplaceFileViewModel, ReplacingFileResultViewModel?> ShowReplaceFileDialog { get; }
        public Interaction<FileHierarchyErrorViewModel, FileHierarchyErrorResultViewModel?> ShowFileHierarchyErrorDialog { get; }
        public Interaction<SameDirectoryErrorViewModel, SameDirectoryErrorResultViewModel?> ShowSameDirectoryErrorDialog { get; }

        // If the file has not been copied, the paste button should be disabled
        private bool _PasteButton_IsEnabled;
        public bool PasteButton_IsEnabled
        {
            get => _PasteButton_IsEnabled;
            set => this.RaiseAndSetIfChanged(ref _PasteButton_IsEnabled, value);
        }

        public bool IsCuted { get; set; } = false;

        // The path to the copied file or directory.
        public string? CopiedPath { get; set; }

        public LogicalOrganizerViewModel(Interaction<DeleteFileViewModel, DeletingFileResultViewModel?> showDeleteFileDialog,
                                         Interaction<ReplaceFileViewModel, ReplacingFileResultViewModel?> showReplaceFileDialog,
                                         Interaction<FileHierarchyErrorViewModel, FileHierarchyErrorResultViewModel?> showFileHierarchyErrorDialog,
                                         Interaction<SameDirectoryErrorViewModel, SameDirectoryErrorResultViewModel?> showSameDirectoryErrorDialog,
                                         ObservableCollection<Node> nodes)
        {
            TryDeleteFile = ReactiveCommand.Create<string>(ExecuteTryDeleteFile);
            CopyFile = ReactiveCommand.Create<string>(ExecuteCopyFile);
            PasteFile = ReactiveCommand.Create<string>(ExecutePasteFile);
            CutFile = ReactiveCommand.Create<string>(ExecuteCutFile);
            CopyPath = ReactiveCommand.Create<string>(ExecuteCopyPath);

            // Bindings to objects that are created in the MainWindowViewModel.
            ShowDeleteFileDialog = showDeleteFileDialog;
            ShowReplaceFileDialog = showReplaceFileDialog;
            ShowFileHierarchyErrorDialog = showFileHierarchyErrorDialog;
            ShowSameDirectoryErrorDialog = showSameDirectoryErrorDialog;

            // The paste button must be disabled before the first copy.
            PasteButton_IsEnabled = false;

            Node.SetCommands(nodes, new Dictionary<string, LogicalOrganizerCommand> { {"Copy", new LogicalOrganizerCommand { Command = CopyFile, CommandParameter = "not null" }},
                                                                                      {"Paste", new LogicalOrganizerCommand { Command = PasteFile, CommandParameter = "not null" }},
                                                                                      {"Open", new LogicalOrganizerCommand { Command = TryDeleteFile, CommandParameter = "not null" }}, });

            LogicalOrganizer = nodes;
        }

        private async void ExecuteTryDeleteFile(string deletePath)
        {
            DeleteFileViewModel deleteFileViewModel = new();

            DeletingFileResultViewModel interactionResult = await ShowDeleteFileDialog.Handle(deleteFileViewModel);

            if (interactionResult != null && interactionResult.IsDelete) 
            {
                DeleteFile(deletePath);
            }
        }

        private void DeleteFile(string deletePath)
        {
            bool isDeleted = FileWorker.DeleteFile(deletePath);

            if (isDeleted && deletePath == CopiedPath)
            {
                PasteButton_IsEnabled = false;
                IsCuted = false;
                CopiedPath = null;
            }

            if (isDeleted && LogicalOrganizer != null && LogicalOrganizer[0].Path != null)
                if (deletePath != LogicalOrganizer[0].Path)
                {
                    LogicalOrganizerRefresh();
                }
                else
                    // The root folder of the logical organizer has been deleted.
                    LogicalOrganizer = null;
        }

        private void ExecuteCopyFile(string copyPath)
        {
            if (IsCuted) IsCuted = false;

            CopiedPath = copyPath;

            PasteButton_IsEnabled = true;
        }

        private async void ExecutePasteFile(string pastePath)
        {
            if (CopiedPath == null)
                return;

            FileInfo copiedPathInfo = new(CopiedPath);

            FileInfo pastePathInfo = new(pastePath);

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

            pastePathInfo = new FileInfo(pastePath);

            bool isSameDirectory = false;

            if (File.GetAttributes(CopiedPath) == FileAttributes.Directory) // This is the directory.
            {
                if (!Directory.Exists(pastePath))
                {
                    try
                    {
                        FileSystem.CopyDirectory(CopiedPath, pastePath);
                    }
                    catch (InvalidOperationException)
                    {
                        FileHierarchyErrorViewModel fileHierarchyErrorViewModel = new();

                        await ShowFileHierarchyErrorDialog.Handle(fileHierarchyErrorViewModel);
                    }
                }
                else if (pastePathInfo.DirectoryName == copiedPathInfo.DirectoryName)
                {
                    isSameDirectory = true;

                    if (!IsCuted)
                    {
                        pastePath = FileWorker.GenerateUniqueDirectoryName(CopiedPath, pastePath);

                        try
                        {
                            FileSystem.CopyDirectory(CopiedPath, pastePath);
                        }
                        catch (InvalidOperationException)
                        {
                            FileHierarchyErrorViewModel fileHierarchyErrorViewModel = new();

                            await ShowFileHierarchyErrorDialog.Handle(fileHierarchyErrorViewModel);
                        }
                    }  
                }
                else
                {
                    ReplaceFileViewModel replaceFileViewModel = new();

                    ReplacingFileResultViewModel interactionResult = await ShowReplaceFileDialog.Handle(replaceFileViewModel);

                    if (interactionResult != null && interactionResult.IsReplace)
                    {
                        try
                        {
                            FileSystem.CopyDirectory(CopiedPath, pastePath, true);
                        }
                        catch (InvalidOperationException)
                        {
                            FileHierarchyErrorViewModel fileHierarchyErrorViewModel = new();

                            await ShowFileHierarchyErrorDialog.Handle(fileHierarchyErrorViewModel);
                        }
                    }
                    else
                    {
                        pastePath = FileWorker.GenerateUniqueDirectoryName(CopiedPath, pastePath);

                        try
                        {
                            FileSystem.CopyDirectory(CopiedPath, pastePath);
                        }
                        catch (InvalidOperationException)
                        {
                            FileHierarchyErrorViewModel fileHierarchyErrorViewModel = new();

                            await ShowFileHierarchyErrorDialog.Handle(fileHierarchyErrorViewModel);
                        }
                    }
                }              
            }
            else // This is a file.
            {
                if (!pastePathInfo.Exists)
                {
                    File.Copy(CopiedPath, pastePath);
                }
                else if (pastePathInfo.DirectoryName == copiedPathInfo.DirectoryName)
                {
                    isSameDirectory = true;

                    if (!IsCuted)
                    {
                        pastePath = FileWorker.GenerateUniqueFileName(CopiedPath, pastePath);

                        File.Copy(CopiedPath, pastePath);
                    }     
                }
                else
                {
                    ReplaceFileViewModel replaceFileViewModel = new();

                    ReplacingFileResultViewModel interactionResult = await ShowReplaceFileDialog.Handle(replaceFileViewModel);

                    if (interactionResult != null && interactionResult.IsReplace)
                    {
                        File.Copy(CopiedPath, pastePath, true);
                    }
                    else
                    {
                        pastePath = FileWorker.GenerateUniqueFileName(CopiedPath, pastePath);

                        File.Copy(CopiedPath, pastePath);
                    }
                }
            }

            if (IsCuted && !isSameDirectory)
            {
                IsCuted = false;

                string cutedPath = CopiedPath;
                CopiedPath = pastePath;

                DeleteFile(cutedPath);
            }
            else if (IsCuted && isSameDirectory) 
            {
                SameDirectoryErrorViewModel sameDirectoryErrorViewModel = new();

                await ShowSameDirectoryErrorDialog.Handle(sameDirectoryErrorViewModel);

                return;
            }

            LogicalOrganizerRefresh();
        }

        private void LogicalOrganizerRefresh()
        {
            if (LogicalOrganizer != null)
            {
                // A list of file path strings whose nodes were expanded before deletion.
                List<string> allExpandedNodes = new();

                // Traversing through all nodes and finding all open nodes.
                NodeWorker.FindAllExpandedNodes(LogicalOrganizer, allExpandedNodes);

                // Creating a new file tree after deletion.
                //LogicalOrganizer = new ObservableCollection<Node>() { new Node(LogicalOrganizer[0].Path, true, allExpandedNodes) };
            }      
        }

        private void ExecuteCutFile(string cutPath)
        {
            CopiedPath = cutPath;

            IsCuted = true;

            PasteButton_IsEnabled = true;
        }

        private void ExecuteCopyPath(string copyPath)
        {
            TextCopy.ClipboardService.SetText(copyPath);

            CopiedPath = null;
            IsCuted = false;
            PasteButton_IsEnabled = false;
        }

        /*
        private void ExecuteOpenFile(string openPath)
        {
            FileInfo openPathInfo = new(openPath);

            if (openPathInfo.Extension == ".json") 
            {
                ObservableCollection.ValuesPath = openPath;
                CentralItems["Value editor"].Content = new ValueEditorViewModel((IDialogService)new DialogService(new DialogManager(viewLocator: new ViewLocator(), dialogFactory: new DialogFactory().AddMessageBox()), viewModelFactory: x => Locator.Current.GetService(x)));
                CentralContent.Clear();
                CentralContent.Add(CentralItems["Value editor"]);
            }
        }*/
    }
}
