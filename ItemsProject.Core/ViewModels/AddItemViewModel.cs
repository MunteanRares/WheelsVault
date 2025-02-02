﻿using ItemsProject.Core.Data;
using ItemsProject.Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ItemsProject.Core.Messages;
using System.Windows.Input;
using ItemsProject.Core.Services;
using ItemsProject.Core.Commands;
using ItemsProject.Core.Commands.AddItemViewModelCommands;
using MvvmCross;

namespace ItemsProject.Core.ViewModels
{
    public class AddItemViewModel : MvxViewModel<FolderModel>
    {
        private readonly IDataService _dataService;
        private readonly IMvxNavigationService _nav;

        public AddItemViewModel(IMvxNavigationService nav, IDataService dataService)
        {
            _dataService = dataService;
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
            _dataService.AddItem(SelectedFolderId, ModelName, ModelReleaseDate, CollectionName);
            _nav.Close(this);
        }

        // VALIDATIONS
        public bool CanAddItem => ModelName?.Length > 0 && ModelReleaseDate?.Length > 0 && CollectionName?.Length > 0;


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
