using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Data.Async.Helpers;
using DevExpress.DirectX.Common.Direct2D;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using MvvmCross.Base;
using MvvmCross.Navigation;

namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class OpenConfirmationWindow : CommandBase
    {
        private readonly IDataService _dataService;
        private readonly Func<string, string> _getMessage;
        private readonly string _title;
        private readonly string _iconSource;

        public OpenConfirmationWindow(IDataService dataService, Func<string, string> getMessage, string title, string iconSource)
        {
            _dataService = dataService;
            _getMessage = getMessage;
            _title = title;
            _iconSource = iconSource;
        }

        public override void Execute(object? parameter)
        {
            if ((FolderModel?)parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            else
            {
                FolderModel folder = (FolderModel)parameter;
                MessageBoxModel parameters = new MessageBoxModel()
                {
                    Message = _getMessage(folder.Name),
                    Title = _title,
                    IconSource = _iconSource,
                    FolderToDelete = folder
                };

                _dataService.NavigatToCustomMessageBoxViewModel(parameters);
            }
        }
    }
}
