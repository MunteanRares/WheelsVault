using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Models
{
    public class ItemModel : MvxNotifyPropertyChanged
    {
        public int Id { get; set; }
        public int FolderId { get; set; }

        private string _modelName;
        public string ModelName
        {
            get { return _modelName; }
            set
            {
                SetProperty(ref _modelName, value);
            }
        }

        private string _modelReleaseDate;
        public string ModelReleaseDate
        {
            get { return _modelReleaseDate; }
            set
            {
                SetProperty(ref _modelReleaseDate, value);
            }
        }

        private string _collectionName;
        public string CollectionName
        {
            get { return _collectionName; }
            set
            {
                SetProperty(ref _collectionName, value);
            }
        }


        private bool _isEditing;
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
