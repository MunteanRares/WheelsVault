﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Data.Filtering.Helpers;
using ItemsProject.Core.Commands.BaseViewModelCommands;
using ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands;
using ItemsProject.Core.Commands.BaseViewModelCommands.Item_Commands;
using ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Messages.HomePage_Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Base;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class HwListCollectionViewModel : MvxViewModel<LoadListCollectionPrepareModel>
    {
        private readonly IDataService _dataService;

        private List<ItemModel> _allFolderItems = new List<ItemModel>();
        private List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();
        private readonly IMvxMessenger _messenger;
        private readonly IMvxMainThreadAsyncDispatcher _thread;

        public HwListCollectionViewModel(IDataService dataService, IMvxMessenger messenger, IMvxMainThreadAsyncDispatcher thread)
        {
            _dataService = dataService;
            _messenger = messenger;
            _thread = thread;

            _tokens.Add(_messenger.Subscribe<AddedHwMessage>(OnAddedHwMessage));
            _tokens.Add(_messenger.Subscribe<UpdateFolderItemsMessage>(OnUpdateFolderItemsMessage));
            _tokens.Add(_messenger.Subscribe<AddCurrentItemMessage>(OnAddCurrentItemMessage));

            FolderItems = new ObservableCollection<ItemModel>();

            // Opening Commands
            OpenPopupCommand = new OpenPopupCommand(SetSelectedItemFolderIds, SetIsCheckedIfItemInFolder);

            // Item Commands
            DeleteItemFromFolderCommand = new DeleteItemFromFolder(_dataService, ExecuteUpdateFolderItems, () => FolderItems, () => SelectedFolder, _messenger);
            DeleteAllItemsCommand = new DeleteAllItemsCommand(_dataService, ExecuteUpdateFolderItems, () => FolderItems, _messenger);
            ToggleItemInFolder = new ToggleItemInFolder(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems);

            // HotWheels Commands
            AddHotWheelsCommand = new AddHotWheelsCommand(_dataService, () => SelectedFolder, _messenger);
            RemoveOneQuantityCommand = new RemoveOneQuantityCommand(_dataService, UpdateFolders);
        }

        // Opening Commands
        public ICommand OpenPopupCommand { get; }

        // Item Commands
        public ICommand CancelItemEditCommand { get; }
        public ICommand DeleteAllItemsCommand { get; }
        public ICommand DeleteItemFromFolderCommand { get; }
        public ICommand EditModeItemCommand { get; }
        public ICommand LoseItemFocusCommand { get; }
        public ICommand RemoveOneQuantityCommand { get; }
        public ICommand SaveEditItemCommand { get; }
        public ICommand ToggleItemInFolder { get; }


        // HotWheels Commands
        public ICommand AddHotWheelsCommand { get; }

        /// <summary>
        /// ////////// VALIDATIONS
        /// </summary>
        public bool CanSaveItemEdit => !string.IsNullOrWhiteSpace(EditingItemName) && !string.IsNullOrWhiteSpace(EditingItemReleaseDate) && !string.IsNullOrWhiteSpace(EditingItemCollectionName);


        /// <summary>
        /// ========  MESSAGES
        /// </summary>
        /// <param name="message"></param>
        private void OnAddedHwMessage(AddedHwMessage message)
        {
            _ = UpdateFolders(message.NewItem);
        }

        private void OnAddCurrentItemMessage(AddCurrentItemMessage message)
        {

            _ = UpdateFolders(message.NewItem);
        }
        

        /// <summary>
        /// ========= FUNCTIONS
        /// </summary>
        /// <param name="parameter"></param>
        public async override void Prepare(LoadListCollectionPrepareModel parameter)
        {
            await AsyncDispatcher.ExecuteOnMainThreadAsync(() => SelectedFolder = parameter.SelectedFolder);
            Folders = parameter.Folders;
            _allFolderItems = await _dataService.LoadItemsForFolder(SelectedFolder);

            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                FolderItems = _dataService.UpdateFolderItems(_allFolderItems, FolderItems);
            });
        }

        public async Task SetSelectedItemFolderIds(ItemModel passedItemModel)
        {
            List<int> folderIds = await _dataService.GetFolderIdsForItem(passedItemModel.Id);
            SelectedItem = passedItemModel;
            SelectedItem.FolderIds = folderIds;
            SetIsCheckedIfItemInFolder(passedItemModel);
        }

        public void SetIsCheckedIfItemInFolder(ItemModel passedItemModel)
        {
            foreach (FolderModel folder in Folders)
            {
                if (passedItemModel.FolderIds.Contains(folder.Id))
                {
                    folder.IsChecked = true;
                }
                else
                {
                    folder.IsChecked = false;
                }
            }
        }

        private async Task UpdateFolders(ItemModel newItem)
        {
            if (newItem.Quantity == 1)
            {
                _allFolderItems.Add(newItem);
                FolderItems.Add(newItem);
            }

            FolderItems =  _dataService.UpdateFolderItems(await _dataService.LoadItemsForFolder(SelectedFolder), FolderItems);
        }

        public  void ExecuteUpdateFolderItems(List<ItemModel> updatedItems)
        {
            _allFolderItems = updatedItems;
            FolderItems =  _dataService.UpdateFolderItems(updatedItems, FolderItems);
        }

        private void UnsubscribeMessages()
        {
            foreach (var token in _tokens)
            {
                token.Dispose();
            }

            _tokens.Clear();
        }

        private void OnUpdateFolderItemsMessage(UpdateFolderItemsMessage message)
        {
            if (message.MethodOption == "UpdateFolderItems")
            {
                List<ItemModel> updatedItems = message.Parameter as List<ItemModel>;
                AsyncDispatcher.ExecuteOnMainThreadAsync(() => FolderItems =  _dataService.UpdateFolderItems(updatedItems, FolderItems));
            }
            else if (message.MethodOption == "SortItems")
            {
                string selectedSortOption = message.Parameter as string;
                AsyncDispatcher.ExecuteOnMainThreadAsync(() => FolderItems =  _dataService.SortItems(selectedSortOption, _allFolderItems, FolderItems));
            }
            else if (message.MethodOption == "SearchItems")
            {
                string searchText = message.Parameter as string;
                List<ItemModel> searchResult = _dataService.FilterItems(searchText, _allFolderItems);
                AsyncDispatcher.ExecuteOnMainThreadAsync(() => FolderItems =  _dataService.UpdateFolderItems(searchResult, FolderItems));
            }
        }

        public void SaveItemEdit()
        {
            SelectedItem.ModelName = EditingItemName.Capitalize();
            SelectedItem.YearProduced = EditingItemReleaseDate;
            SelectedItem.SeriesName = EditingItemCollectionName.ToUpper();
            SelectedItem.IsEditing = false;
        }

        public void CancelEdit<T>(T model, Action<T> revertEditAction, Action<bool> setEditingFlag)
        {
            revertEditAction(model);
            setEditingFlag(false);
        }
        public void CancelItemEditing(ItemModel passedItemModel)
        {
            CancelEdit<ItemModel>(
                passedItemModel,
                model =>
                {
                    EditingItemName = model.ModelName;
                    EditingItemReleaseDate = model.YearProduced;
                    EditingItemCollectionName = model.SeriesName;
                },
                (flag) =>
                {
                    SelectedItem.IsEditing = flag;
                }
            );
        }

        public void BeginItemEdit(ItemModel selectedItem)
        {
            EditingItemName = selectedItem.ModelName;
            EditingItemReleaseDate = selectedItem.YearProduced;
            EditingItemCollectionName = selectedItem.SeriesName;
        }

        public void EditModeItems(ItemModel selectedItem, bool value)
        {
            if (value)
            {
                BeginItemEdit(selectedItem);
            }
            SelectedItem = selectedItem;
            selectedItem.IsEditing = value;
        }

        private ItemModel? _selectedItem;
        public ItemModel? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        private ObservableCollection<FolderModel> _folders;
        public ObservableCollection<FolderModel> Folders
        {
            get { return _folders; }
            set
            {
                SetProperty(ref _folders, value);
            }
        }

        private string _editingItemName;
        public string EditingItemName
        {
            get { return _editingItemName; }
            set
            {
                SetProperty(ref _editingItemName, value);
                RaisePropertyChanged(() => CanSaveItemEdit);
            }
        }

        private string _editingItemReleaseDate;
        public string EditingItemReleaseDate
        {
            get { return _editingItemReleaseDate; }
            set
            {
                SetProperty(ref _editingItemReleaseDate, value);
                RaisePropertyChanged(() => CanSaveItemEdit);
            }
        }

        private string _editingItemCollectionName;
        public string EditingItemCollectionName
        {
            get { return _editingItemCollectionName; }
            set
            {
                SetProperty(ref _editingItemCollectionName, value);
                RaisePropertyChanged(() => CanSaveItemEdit);
            }
        }

        private FolderModel _selectedFolder;
        public FolderModel SelectedFolder
        {
            get { return _selectedFolder; }
            set 
            { 
                SetProperty(ref _selectedFolder, value);    
            }
        }

        private ObservableCollection<ItemModel> _folderItems;
        public ObservableCollection<ItemModel> FolderItems
        {
            get { return _folderItems; }
            set 
            { 
                SetProperty(ref _folderItems, value);   
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            UnsubscribeMessages();
            base.ViewDestroy(viewFinishing);
        }

        public override void ViewDisappeared()
        {
            UnsubscribeMessages();
            base.ViewDisappeared();
        }
    }
}
