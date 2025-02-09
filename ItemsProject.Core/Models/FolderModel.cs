

using DevExpress.Data.Browsing.Design;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Models
{
    public class FolderModel : MvxNotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private bool _isEditing = false;
        public bool IsEditing
        {
            get { return _isEditing; }
            set 
            { 
                SetProperty(ref _isEditing, value);
            }
        }

    }
}
