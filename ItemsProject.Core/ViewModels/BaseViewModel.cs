using System.Collections.ObjectModel;
using System.Windows.Input;
using ItemsProject.Core.Commands;
using ItemsProject.Core.Data;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using Microsoft.Identity.Client.Extensibility;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
		private readonly IDatabaseData _db;
		private readonly IMvxNavigationService _navigation;
		private readonly IDataService _dataService;
		private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

		private List<ItemModel> _allFolderItems = new List<ItemModel>();
        private List<ItemModel> _searchResult = new List<ItemModel>();

        public BaseViewModel(IDatabaseData db, IDataService dataService, IMvxNavigationService navigation, IMvxMessenger messenger)
        {
			_db = db;
			_navigation = navigation;
			_dataService = dataService;
            Folders = new ObservableCollection<FolderModel>(_db.GetAllFolderItems());
			FolderItems = new ObservableCollection<ItemModel>();

            // Messages
            _tokens.Add(messenger.Subscribe<AddedItemMessage>(OnAddedItemMessage));
            _tokens.Add(messenger.Subscribe<AddedFolderMessage>(OnAddedFolderMessage));

			// Commands
            openAddItemWindowCommand = new MvxCommand(OpenAddItemWindow);
			openAddFolderWindowCommand = new MvxCommand(OpenAddFolderWindow);
			DeleteItemFromFolderCommand = new DeleteItemFromFolder(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems);
		}

		// COMMANDS
		public IMvxCommand openAddItemWindowCommand { get; set; }
        public IMvxCommand openAddFolderWindowCommand { get; set; }
		public ICommand DeleteItemFromFolderCommand { get; set; }

        public void OpenAddItemWindow()
		{
			SearchText = string.Empty;
			_navigation.Navigate<AddItemViewModel, FolderModel>(SelectedFolder);
        }

		public void OpenAddFolderWindow()
		{
			_navigation.Navigate<AddFolderViewModel>();
		}

		public void ExecuteUpdateFolderItems(List<ItemModel> updatedItems)
		{
			_allFolderItems = updatedItems;
			FolderItems = _dataService.UpdateFolderItems(updatedItems, FolderItems);
		}


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


        // VALIDATIONS
        public bool CanPressAddItem => SelectedFolder != null;


        // PROPERTIES
        public ObservableCollection<ItemModel> FolderItems { get; set; }
        public ObservableCollection<FolderModel> Folders { get; }
		

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
	}
}
