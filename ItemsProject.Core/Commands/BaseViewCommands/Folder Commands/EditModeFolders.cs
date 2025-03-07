﻿using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class EditModeFolders : CommandBase
    {
        private readonly Action<FolderModel, bool> _changeEditMode;
        public EditModeFolders(Action<FolderModel, bool> changeEditMode)
        {
            _changeEditMode = changeEditMode;
        }

        public override void Execute(object? parameter)
        {
            FolderModel folderModel = parameter as FolderModel;
            _changeEditMode(folderModel, !folderModel.IsEditing);
        }
    }
}
