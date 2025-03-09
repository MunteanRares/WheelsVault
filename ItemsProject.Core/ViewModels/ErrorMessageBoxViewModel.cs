using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemsProject.Core.Commands.General;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public  class ErrorMessageBoxViewModel : MvxViewModel<string>
    {
        private readonly IMvxNavigationService _nav;

        public ErrorMessageBoxViewModel(IMvxNavigationService nav)
        {
            _nav = nav;
            CloseWarningMessageCommand = new Cancel(CloseWarningMessage);
        }

        public ICommand CloseWarningMessageCommand { get; }

        /// <summary>
        /// ========= FUNCTIONS
        /// </summary>
        public override void Prepare(string parameter)
        {
            ErrorMessage = parameter;
        }

        public void CloseWarningMessage()
        {
            _nav.Close(this);
        }

        /// <summary>
        /// ========  PROPERTIESs
        /// </summary>
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }
    }
}
