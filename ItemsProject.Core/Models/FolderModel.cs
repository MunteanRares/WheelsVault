

using DevExpress.Data.Browsing.Design;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Models
{
    public class FolderModel : MvxNotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

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
