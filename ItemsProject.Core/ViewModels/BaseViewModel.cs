﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ItemsProject.Core.Commands.BaseViewModelCommands;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands;
using ItemsProject.Core.Commands.BaseViewModelCommands.Item_Commands;


namespace ItemsProject.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
		private readonly IDatabaseData _db;
		private readonly IMvxNavigationService _nav;
		private readonly IDataService _dataService;
		private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

		private List<ItemModel> _allFolderItems = new List<ItemModel>();
        private List<ItemModel> _searchResult = new List<ItemModel>();

		public BaseViewModel(IMvxNavigationService nav, IDatabaseData db, IDataService dataService, IMvxMessenger messenger)
		{
			_db = db;
			_dataService = dataService;
			_nav = nav;

			Folders = new ObservableCollection<FolderModel>(_db.GetAllFolders());
			FolderItems = new ObservableCollection<ItemModel>();

			// Messages
			_tokens.Add(messenger.Subscribe<AddedItemMessage>(OnAddedItemMessage));
			_tokens.Add(messenger.Subscribe<AddedFolderMessage>(OnAddedFolderMessage));
			_tokens.Add(messenger.Subscribe<CanRemoveFolderMessage>(OnRemoveFolderMessage));
			_tokens.Add(messenger.Subscribe<ChangeWindowStateMessage>(OnChangeWindowStateMessage));

			// COMMANDS
			// Opening Commands
			OpenAddItemWindowCommand = new OpenAddItemWindow(_nav, () => SelectedFolder, ClearSearchText, SetWindowStateToFalse);
			OpenAddFolderWindowCommand = new OpenAddFolderWindow(_nav, SetWindowStateToFalse);
			OpenDeleteFolderConfirmationCommand = new OpenConfirmationWindow(_nav, DeleteFolderConfirmationMessage, "Confirm Deletion", "pack://application:,,,/Assets/Icons/question-mark.png", SetWindowStateToFalse);
			OpenPopupCommand = new OpenPopupCommand(_dataService, SetSelectedItemFolderIds, SetIsCheckedIfItemInFolder);

            // Folder Commands
            DeleteFolderCommand = new DeleteFolder(_dataService, ExecuteFolderRemoved, () => Folders.ToList());
            EditModeFoldersCommand = new EditModeFolders(ChangeFolderEditMode);
            CancelFolderEditCommand = new CancelFolderEdit(CancelFolderEditing);
            SaveEditFolderCommand = new SaveEditFolder(_dataService, () => EditingFolderName, SaveFolderEdit);

			// Item Commands
            DeleteItemFromFolderCommand = new DeleteItemFromFolder(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems, () => SelectedFolder);
            EditModeItemCommand = new EditItemFromFolder(EditModeItems);
            CancelItemEditCommand = new CancelItemEdit(CancelItemEditing);		
			SaveEditItemCommand = new SaveEditItem(_dataService, () => EditingItemName, () => EditingItemReleaseDate, () => EditingItemCollectionName, SaveItemEdit);
			LoseItemFocusCommand = new CancelItemEditingCommand(_dataService);
            ToggleItemInFolder = new ToggleItemInFolder(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems);
			DeleteAllItemsCommand = new DeleteAllItemsCommand(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems);

            // Setting Default Values
            SelectedSortOption = SortOptions[0];
			Folders[0].IsDefault = true;
        }

        /// <summary>
		/// COLLECTION OF COMMAND DECLARATION
		/// </summary>
		// Opening Commands
        public ICommand OpenAddItemWindowCommand { get; }
		public ICommand OpenAddFolderWindowCommand { get; }
        public ICommand OpenDeleteFolderConfirmationCommand { get; }
        public ICommand OpenPopupCommand { get; }

        // Folder Commands
        public ICommand DeleteFolderCommand { get; }
		public ICommand EditModeFoldersCommand { get; }
        public ICommand CancelFolderEditCommand { get; }
        public ICommand SaveEditFolderCommand { get; }

        // Item Commands
        public ICommand DeleteItemFromFolderCommand { get; }
        public ICommand EditModeItemCommand { get; }        
		public ICommand CancelItemEditCommand { get; }
		public ICommand SaveEditItemCommand { get; }
		public ICommand LoseItemFocusCommand { get; }
        public ICommand ToggleItemInFolder { get; }
		public ICommand DeleteAllItemsCommand { get; }
		public ICommand OpenDeleteAllItemsFromFolderCommand { get; }


        /// <summary>
		///	FUNCTIONS THAT CALL WHENEVER THIS VIEWMODEL GETS MESSAGES
		/// </summary>
        private void OnAddedItemMessage(AddedItemMessage addedItemMessage)
        {
            _allFolderItems.Add(addedItemMessage.NewItem);
            FolderItems.Add(addedItemMessage.NewItem);
        }

        private void OnAddedFolderMessage(AddedFolderMessage addedFolderMessage)
        {
            Folders.Add(addedFolderMessage.NewFolder);
        }

        private void OnRemoveFolderMessage(CanRemoveFolderMessage message)
        {
			_dataService.ExecuteDeleteFolderCommand(message, DeleteFolderCommand);
        }

		private void OnChangeWindowStateMessage(ChangeWindowStateMessage changeWindowStateMessage)
		{
			IsWindowEnabled = changeWindowStateMessage.ChangeWindowState;
		}

        /// <summary>
		/// GENERAL HELPER FUNCTIONS
		/// </summary>
        private void UnsubscribeMessages()
        {
            foreach (MvxSubscriptionToken token in _tokens)
            {
                token.Dispose();
            }

            _tokens.Clear();
        }

        public void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        public string DeleteFolderConfirmationMessage(string folderName)
        {
            string output = string.Empty;
            output = $"Are you sure you want to delete the '{folderName}' folder?";
            return output;
        }

        public void ExecuteUpdateFolderItems(List<ItemModel> updatedItems)
        {
            _allFolderItems = updatedItems;
            FolderItems = _dataService.UpdateFolderItems(updatedItems, FolderItems);
        }

        public void ExecuteFolderRemoved(List<FolderModel> updatedFolders)
        {
            Folders = _dataService.UpdateFolders(updatedFolders, Folders);
        }

        public void SetWindowStateToFalse()
        {
            IsWindowEnabled = false;
        }

        public void ChangeEditMode<T>(T passedModel, bool isEditing, Action<T> setEditAction, Action<T, bool> setEditingFlag)
        {
            if (isEditing)
            {
                setEditAction(passedModel);
            }

            setEditingFlag(passedModel, isEditing);
        }

        public void CancelEdit<T>(T model, Action<T> revertEditAction, Action<bool> setEditingFlag)
        {
            revertEditAction(model);
            setEditingFlag(false);
        }

        // FUNCTIONS - FOLDER EDITING
		public void ChangeFolderEditMode(FolderModel passedFolder, bool isEditing)
		{
			ChangeEditMode<FolderModel>(
				passedFolder,
				isEditing,
				model => 
				{ 
					EditingFolderName = model.Name; 
				}, 
				(model, isEditing) => 
				{
					SelectedFolder = model;
					model.IsEditing = isEditing;
				});
		}

        public void CancelFolderEditing(FolderModel passedFolderModel)
		{
			CancelEdit<FolderModel>(
				passedFolderModel,
				model =>
				{
					EditingFolderName = model.Name;
				},
				(flag) =>
				{
					SelectedFolder.IsEditing = flag;
				});
		}

		public void SaveFolderEdit()
		{
			SelectedFolder.Name = EditingFolderName.Capitalize();
			SelectedFolder.IsEditing = false;
		}

		// FUNCTIONS - ITEM EDITING
		public void EditModeItems(ItemModel selectedItem, bool value)
		{
            if (value)
            {
				BeginItemEdit(selectedItem);
            }
			SelectedItem = selectedItem;
			selectedItem.IsEditing = value;
        }

		public void BeginItemEdit(ItemModel selectedItem)
		{
			EditingItemName = selectedItem.ModelName;
            EditingItemReleaseDate = selectedItem.ModelReleaseDate;
            EditingItemCollectionName = selectedItem.CollectionName;
        }

		public void CancelItemEditing(ItemModel passedItemModel)
		{
			CancelEdit<ItemModel>(
				passedItemModel,
				model =>
				{
					EditingItemName = model.ModelName;
					EditingItemReleaseDate = model.ModelReleaseDate;
					EditingItemCollectionName = model.CollectionName;
				},
				(flag) =>
				{
					SelectedItem.IsEditing = flag;
				}
			);
		}

		public void SetSelectedItemFolderIds(ItemModel passedItemModel)
		{
			List<int> folderIds = _dataService.GetFolderIdsForItem(passedItemModel.Id);
			SelectedItem = passedItemModel;
            SelectedItem.FolderIds = folderIds;
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

		public void SaveItemEdit()
		{
			SelectedItem.ModelName = EditingItemName.Capitalize();
			SelectedItem.ModelReleaseDate = EditingItemReleaseDate;
			SelectedItem.CollectionName = EditingItemCollectionName.ToUpper();
			SelectedItem.IsEditing = false;
		}

        /// <summary>
		/// LIST OF VALIDATION FUNCTIONS THAT CHANGE HOW THE UI RESPONDS
		/// </summary>
        public bool IsFolderSelected => SelectedFolder != null;
		public bool CanSaveItemEdit => !string.IsNullOrWhiteSpace(EditingItemName) && !string.IsNullOrWhiteSpace(EditingItemReleaseDate) && !string.IsNullOrWhiteSpace(EditingItemCollectionName);
		public bool CanSaveFolderEdit => !string.IsNullOrWhiteSpace(EditingFolderName);
		public bool IsFirstFolderSelected => SelectedFolder.Name == "All Cars";


        /// <summary>
		///	BASE VIEWMODEL PROPERTIES
		/// </summary>
        public ObservableCollection<ItemModel> FolderItems { get; private set; }
       
        private ObservableCollection<FolderModel> _folders;
        public ObservableCollection<FolderModel> Folders
        {
            get { return _folders; }
            set
            {
                SetProperty(ref _folders, value);
            }
        }

        public ObservableCollection<string> SortOptions { get; private set; } = new ObservableCollection<string>
		{
			"Date Added",
			"A-Z",
			"Z-A"
		};

		private string _selectedSortOption;
		public string SelectedSortOption
		{
			get { return _selectedSortOption; }
			set 
			{ 
				SetProperty(ref _selectedSortOption, value );
				FolderItems = _dataService.SortItems(SelectedSortOption, _allFolderItems, FolderItems);
			}
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

		private FolderModel _selectedFolder;
		public FolderModel SelectedFolder
		{
			get { return _selectedFolder; }
			set
			{ 
				SetProperty(ref _selectedFolder, value);
				RaisePropertyChanged(() => IsFolderSelected);
				SelectedItem = null;
				SelectedSortOption = SortOptions[0];
				_allFolderItems = _dataService.LoadItemsForFolder(SelectedFolder);
				_searchResult = _dataService.FilterItems(SearchText, _allFolderItems);
                FolderItems = _dataService.UpdateFolderItems(_searchResult, FolderItems);
            }
		}

		private string _searchText = string.Empty;
		public string SearchText
		{
			get { return _searchText; }
			set 
			{ 
				SetProperty(ref _searchText, value);
				SelectedItem = null;
				_searchResult = _dataService.FilterItems(SearchText, _allFolderItems);
                FolderItems = _dataService.UpdateFolderItems(_searchResult, FolderItems);
            }
		}

		private bool _isWindowEnabled = true;
		public bool IsWindowEnabled
        {
			get { return _isWindowEnabled; }
			set 
			{
				SetProperty(ref _isWindowEnabled, value);
			}
		}

		private string _editingFolderName;
		public string EditingFolderName
		{
			get { return _editingFolderName; }
			set 
			{
				SetProperty(ref _editingFolderName, value);
				RaisePropertyChanged(() => CanSaveFolderEdit);
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

		// WHEN CLOSING APP
		public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            UnsubscribeMessages();
        }
    }
}
