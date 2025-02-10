﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Models;

namespace ItemsProject.Core.Commands.General
{
    public class CancelFolderEdit : CommandBase
    {
        private readonly Action<FolderModel> _onLostFocusAction;

        public CancelFolderEdit(Action<FolderModel> onLostFocusAction)
        {
            _onLostFocusAction = onLostFocusAction;
        }

        public override void Execute(object? parameter)
        {
            _onLostFocusAction((FolderModel)parameter);
        }
    }
}
