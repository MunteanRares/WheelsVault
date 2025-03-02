﻿using ItemsProject.Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Windows.Input;
using ItemsProject.Core.Services;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Commands.AddItemViewModelCommands;


namespace ItemsProject.Core.ViewModels
{
    public class AddItemViewModel : MvxViewModel<FolderModel>
    {
        private readonly IMvxNavigationService _nav;
        private readonly IItemDataService _itemDataService;

        public AddItemViewModel(IMvxNavigationService nav, IItemDataService itemDataService)
        {
            _itemDataService = itemDataService;
            _nav = nav;

            CancelCommand = new Cancel(CloseWindow);
            AddItemCommand = new AddItemConfirm(ConfirmAddItem);
        }

        public override void Prepare(FolderModel parameter)
        {
            SelectedFolderId = parameter.Id;
        }

        // COMMANDS
        public ICommand CancelCommand { get; }
        public ICommand AddItemCommand { get; }

        // Commands Functionalities
        public void CloseWindow()
        {
            _nav.Close(this);
        }

        public void ConfirmAddItem()
        {
            _itemDataService.AddItem(SelectedFolderId, ModelName, ModelReleaseDate, CollectionName);
            _nav.Close(this);
        }

        // VALIDATIONS
        public bool CanAddItem => !string.IsNullOrWhiteSpace(ModelName) && !string.IsNullOrWhiteSpace(ModelReleaseDate) && !string.IsNullOrWhiteSpace(CollectionName);

        // PROPERTIES
        private int _selectedFolderId;
        public int SelectedFolderId
        {
            get { return _selectedFolderId; }
            set 
            { 
                SetProperty(ref _selectedFolderId, value);
            }
        }

        private string _modelName;
        public string ModelName
        {
            get { return _modelName; }
            set 
            { 
                SetProperty(ref _modelName, value);
                RaisePropertyChanged(() => CanAddItem);
            }
        }

        private string _modelReleaseDate;
        public string ModelReleaseDate
        {
            get { return _modelReleaseDate; }
            set
            {
                SetProperty(ref _modelReleaseDate, value);
                RaisePropertyChanged(() => CanAddItem);
            }
        }

        private string _collectionName;
        public string CollectionName
        {
            get { return _collectionName; }
            set 
            { 
                SetProperty(ref _collectionName, value);
                RaisePropertyChanged(() => CanAddItem);
            }
        }
    }
}
