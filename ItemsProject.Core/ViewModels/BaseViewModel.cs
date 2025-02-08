using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ItemsProject.Core.Commands.BaseViewModelCommands;
using System.Runtime.CompilerServices;
using System.Runtime;
using DevExpress.Data;
using Microsoft.VisualBasic;


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

			Folders = new ObservableCollection<FolderModel>(_db.GetAllFolderItems());
			FolderItems = new ObservableCollection<ItemModel>();

			// Messages
			_tokens.Add(messenger.Subscribe<AddedItemMessage>(OnAddedItemMessage));
			_tokens.Add(messenger.Subscribe<AddedFolderMessage>(OnAddedFolderMessage));
			_tokens.Add(messenger.Subscribe<CanRemoveFolderMessage>(OnRemoveFolderMessage));
			_tokens.Add(messenger.Subscribe<ChangeWindowStateMessage>(OnChangeWindowStateMessage));

			// Commands
			OpenAddItemWindowCommand = new OpenAddItemWindow(_nav, () => SelectedFolder, ClearSearchText);
			OpenAddFolderWindowCommand = new OpenAddFolderWindow(_nav);
			DeleteItemFromFolderCommand = new DeleteItemFromFolder(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems);
			DeleteFolderConfirmationCommand = new OpenConfirmationWindow(_nav, DeleteFolderConfirmationMessage, "Confirm Deletion", "pack://application:,,,/Assets/Icons/question-mark.png", SetWindowStateToFalse);
			DeleteFolderCommand = new DeleteFolder(_dataService, ExecuteFolderRemoved, () => Folders.ToList());
			EditModeFoldersCommand = new EditModeFolders(EditModeFolders);
		}

		public string DeleteFolderConfirmationMessage(string folderName)
		{
			string output = string.Empty;
			output = $"Are you sure you want to delete the '{folderName}' folder?";
            return output;
		}

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
			UnsubscribeMessages();
        }

        // COMMANDS
        public ICommand OpenAddItemWindowCommand { get; }
		public ICommand OpenAddFolderWindowCommand { get; }
		public ICommand DeleteItemFromFolderCommand { get; }
		public ICommand DeleteFolderConfirmationCommand { get; }
		public ICommand DeleteFolderCommand { get; }
		public ICommand EditModeFoldersCommand { get; }
        
        // MESSAGES
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

        // FUNCTIONS
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

        public void ExecuteUpdateFolderItems(List<ItemModel> updatedItems)
        {
            _allFolderItems = updatedItems;
            FolderItems = _dataService.UpdateFolderItems(updatedItems, FolderItems);
        }

        public void ExecuteFolderRemoved(List<FolderModel> updatedFolders)
        {
            Folders = _dataService.UpdateFolders(updatedFolders, Folders);
        }

		public void EditModeFolders(FolderModel selectedFolder, bool value)
		{
            SelectedFolder = selectedFolder;
            selectedFolder.IsEditing = value;
        }

		public void OnLostFocusStopEdit()
		{
            SelectedFolder.IsEditing = false;
		}

		public void SetWindowStateToFalse()
		{
			IsWindowEnabled = false;
		}

		public FolderModel GetCurrentSelectedFolder()
		{
			return SelectedFolder;
		}

        // VALIDATIONS
        public bool CanPressAddItem => SelectedFolder != null;

		// PROPERTIES
		public ObservableCollection<ItemModel> FolderItems { get; private set; }
        public ObservableCollection<FolderModel> Folders { get; private set; }
		

		private ItemModel _selectedItem;
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
				RaisePropertyChanged(() => CanPressAddItem);
				SelectedItem = null;
				_allFolderItems =_dataService.LoadItemsForFolder(SelectedFolder);
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

	}
}
