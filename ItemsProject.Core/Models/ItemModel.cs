﻿using MvvmCross.ViewModels;

namespace ItemsProject.Core.Models
{
    public class ItemModel : MvxNotifyPropertyChanged
    {
        public int Id { get; set; }

        private List<int> _folderIds = new List<int>();

        public List<int> FolderIds
        {
            get { return _folderIds; }
            set 
            { 
                SetProperty(ref _folderIds, value); 
            }
        }

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

        private bool _isPopupOpened;
        public bool IsPopupOpened
        {
            get { return _isPopupOpened; }
            set 
            { 
                SetProperty(ref _isPopupOpened, value);
            }
        }

    }
}
