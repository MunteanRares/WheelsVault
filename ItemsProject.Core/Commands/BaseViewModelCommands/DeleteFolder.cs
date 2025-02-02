using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using DevExpress.Utils;
using ItemsProject.Core.Data;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class DeleteFolder : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<List<FolderModel>> _getAllFolders;
        private readonly Action<List<FolderModel>> _upateFolders;
        public DeleteFolder(IDataService dataService, Action<List<FolderModel>> updateFolders, Func<List<FolderModel>> getAllFolders)
        {
            _dataService = dataService;
            _getAllFolders = getAllFolders;
            _upateFolders = updateFolders;
        }

        public override void Execute(object? parameter)
        {
            FolderModel passedInFolder = parameter as FolderModel;

            var allFolders = _getAllFolders();
            FolderModel folderToRemoveCopy = _dataService.RemoveFolder(passedInFolder.Id);
            FolderModel folderToRemove = allFolders.Where(i => i.Id == folderToRemoveCopy.Id).First();

            allFolders.Remove(folderToRemove);

            _upateFolders(allFolders);
        }
    }
}
