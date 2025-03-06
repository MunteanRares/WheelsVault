using System.Collections.ObjectModel;
using System.Windows.Input;
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
using WikiHotWheelsWebScraper.Models;
using Timer = System.Timers.Timer;
using ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands;
using MvvmCross;
using DevExpress.Utils.Serializing.Helpers;
using Nito.AsyncEx;
using ItemsProject.Core.Messages.HwListVm_Messages;


namespace ItemsProject.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _nav;
        private readonly IDataService _dataService;
        private readonly IMvxMessenger _messenger;
        private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

        private List<ItemModel> _allFolderItems = new List<ItemModel>();
        private List<ItemModel> _searchResult = new List<ItemModel>();
        private Timer _debounceTimer;
        private Timer _debounceTimerSearchBox;
        private SynchronizationContext? _uiContext;

        public BaseViewModel(IMvxNavigationService nav, IDataService dataService, IMvxMessenger messenger)
        {
            _dataService = dataService;
            _nav = nav;
            _messenger = messenger;

            Folders = new ObservableCollection<FolderModel>(_dataService.GetAllFolders());

            // Messages
            _tokens.Add(_messenger.Subscribe<AddedFolderMessage>(OnAddedFolderMessage));
            _tokens.Add(_messenger.Subscribe<CanRemoveFolderMessage>(OnRemoveFolderMessage));
            _tokens.Add(_messenger.Subscribe<ChangeWindowStateMessage>(OnChangeWindowStateMessage));
            _tokens.Add(_messenger.Subscribe<ChangeCurrentViewMessage>(OnChangeCurrentViewMessage));
            _tokens.Add(_messenger.Subscribe<WorkCompletedMessage>(OnWorkCompletedMessage));

            // COMMANDS
            // Opening Commands
            OpenAddItemWindowCommand = new OpenAddItemWindow(_nav, () => SelectedFolder, ClearSearchText, SetWindowStateToFalse);
            OpenAddFolderWindowCommand = new OpenAddFolderWindow(_nav, SetWindowStateToFalse);
            OpenDeleteFolderConfirmationCommand = new OpenConfirmationWindow(_nav, DeleteFolderConfirmationMessage, "Confirm Deletion", "pack://application:,,,/Assets/Icons/question-mark.png", SetWindowStateToFalse);
            OpenSettingsCommand = new ToggleSettingsCommand(ToggleSettingsView);
            GoHomePageCommand = new GoHomePageCommand(GoHomeView);


            // Folder Commands
            DeleteFolderCommand = new DeleteFolder(_dataService, ExecuteFolderRemoved, () => Folders.ToList());
            EditModeFoldersCommand = new EditModeFolders(ChangeFolderEditMode);
            CancelFolderEditCommand = new CancelFolderEdit(CancelFolderEditing);
            SaveEditFolderCommand = new SaveEditFolder(_dataService, () => EditingFolderName, SaveFolderEdit);

            //// HotWheels Commands
            AddHotWheelsCommand = new AddHotWheelsCommand(_dataService, () => SelectedFolder, _messenger);

            // Setting Default Values
            SelectedSortOption = SortOptions[0];
            Folders[0].IsDefault = true;
            TotalHotWheelsCount = _dataService.GetAllHotWheelsCount();
            TotalCarsCount = _dataService.GetAllCarsCount();
            AppVersion = "1.1.5.0";

            _uiContext = SynchronizationContext.Current;

            _debounceTimerSearchBox = new Timer(600);
            _debounceTimerSearchBox.Elapsed += (sender, e) => DebounceTimerSearchBox_Tick();
            _debounceTimerSearchBox.AutoReset = false;

            _debounceTimer = new Timer(1000);
            _debounceTimer.Elapsed += (sender, e) => DebounceTimer_Tick();
            _debounceTimer.AutoReset = false;
        }

        /// <summary>
        /// COLLECTION OF COMMAND DECLARATION
        /// </summary>
        /// 
        // Opening Commands
        public ICommand OpenAddItemWindowCommand { get; }
        public ICommand OpenAddFolderWindowCommand { get; }
        public ICommand OpenDeleteFolderConfirmationCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand GoHomePageCommand { get;  }

        // Folder Commands
        public ICommand DeleteFolderCommand { get; }
        public ICommand EditModeFoldersCommand { get; }
        public ICommand CancelFolderEditCommand { get; }
        public ICommand SaveEditFolderCommand { get; }

        //// HotWheels Commands
        public ICommand AddHotWheelsCommand { get; }


        /// <summary>
        //////////////	FUNCTIONS THAT CALL WHENEVER THIS VIEWMODEL GETS MESSAGES
        /// </summary>
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
        private void OnChangeCurrentViewMessage(ChangeCurrentViewMessage message)
        {
            if (message.Sender.GetType() == typeof(SettingsViewModel))
            {
                SelectedFolder = null;
            }

            CurrentView = message.ViewModel;
        }

        private void OnWorkCompletedMessage(WorkCompletedMessage message)
        {
            NotLoadingItems = true;
        }

        /// <summary>
        ///////////// GENERAL HELPER FUNCTIONS
        /// </summary>
        public void CancelEdit<T>(T model, Action<T> revertEditAction, Action<bool> setEditingFlag)
        {
            revertEditAction(model);
            setEditingFlag(false);
        }

        public void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        public void ChangeEditMode<T>(T passedModel, bool isEditing, Action<T> setEditAction, Action<T, bool> setEditingFlag)
        {
            if (isEditing)
            {
                setEditAction(passedModel);
            }

            setEditingFlag(passedModel, isEditing);
        }

        private void DebounceTimer_Tick()
        {
            _debounceTimer.Stop();
            if (SearchhwText != "Add HotWheels..." && SearchhwText.Length > 2)
            {
                SearchhwResult = _dataService.SearchHotWheels(SearchhwText);
            }
            if ((SearchhwText.Length < 3 || string.IsNullOrWhiteSpace(SearchhwText) || SearchhwText == "Add HotWheels...") && SearchhwResult != null)
            {
                if (_uiContext != null)
                {
                    _uiContext.Send(x => SearchhwResult = null, null);
                }
            }
        }

        private void DebounceTimerSearchBox_Tick()
        {
            _debounceTimerSearchBox.Stop();
            _searchResult = _dataService.FilterItems(SearchText, _allFolderItems);
            UpdateFolderItemsMessage message = new UpdateFolderItemsMessage(this, _searchResult, "UpdateFolderItems");
            _messenger.Publish(message);
        }

        public void GoHomeView()
        {
            if (CurrentView is not HomePageViewModel)
            {
                CurrentView = Mvx.IoCProvider.Resolve<HomePageViewModel>();
                if (SelectedFolder != null)
                {
                    SelectedFolder = null;
                }
            }
        }

        public async Task NavigateAndLoadListCollection()
        {
            LoadListCollectionPrepareModel param = new LoadListCollectionPrepareModel(SelectedFolder, Folders);

            if (CurrentView is LoadListCollectionViewModel existingVm)
            {
                existingVm.Prepare(param);
            }

            else
            {
                LoadListCollectionViewModel vm = Mvx.IoCProvider.Resolve<LoadListCollectionViewModel>();
                vm.Prepare(param);
                vm.Initialize();
                CurrentView = vm;
            }
        }

        public void SetWindowStateToFalse()
        {
            IsWindowEnabled = false;
        }

        private void ToggleSettingsView()
        {
            if (CurrentView == null || CurrentView is not SettingsViewModel)
            {
                CurrentView = Mvx.IoCProvider.Resolve<SettingsViewModel>();
            }
            else
            {
                SelectedFolder = null;
                CurrentView = Mvx.IoCProvider.Resolve<HomePageViewModel>();
            }
        }

        private void UnsubscribeMessages()
        {
            foreach (MvxSubscriptionToken token in _tokens)
            {
                token.Dispose();
            }

            _tokens.Clear();
        }

        /// <summary>
        /// /////////// FOLDER FUNCTIONALITIES
        /// </summary>
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

        public string DeleteFolderConfirmationMessage(string folderName)
        {
            string output = string.Empty;
            output = $"Are you sure you want to delete the '{folderName}' folder?";
            return output;
        }

        public void ExecuteFolderRemoved(List<FolderModel> updatedFolders)
        {
            Folders = _dataService.UpdateFolders(updatedFolders, Folders);
        }

        public void ExecuteUpdateFolderItems(List<ItemModel> updatedItems)
        {
            _allFolderItems = updatedItems;
            UpdateFolderItemsMessage message = new UpdateFolderItemsMessage(this, updatedItems, "UpdateFolderItems");
        }

        public void SaveFolderEdit()
        {
            SelectedFolder.Name = EditingFolderName.Capitalize();
            SelectedFolder.IsEditing = false;
        }
        /// <summary>
        /// ////////// ITEM FUNCTIONALITIES
        /// </summary>
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

        /// <summary>
        //////////////LIST OF VALIDATION FUNCTIONS THAT CHANGE HOW THE UI RESPONDS
        /// </summary>
		public bool CanSaveItemEdit => !string.IsNullOrWhiteSpace(EditingItemName) && !string.IsNullOrWhiteSpace(EditingItemReleaseDate) && !string.IsNullOrWhiteSpace(EditingItemCollectionName);
        public bool CanSaveFolderEdit => !string.IsNullOrWhiteSpace(EditingFolderName);
        public bool IsFolderSelected => SelectedFolder != null;
        public bool IsPopupHwOpened => SearchhwResult != null && SearchhwResult.Count > 0;
        public bool IsSettingsViewOpen => CurrentView == null || CurrentView.GetType() != typeof(SettingsViewModel);


        /// <summary>
        //////////////	BASE VIEWMODEL PROPERTIES
        /// </summary>
        private string _appVersion;
        public string AppVersion
        {
            get { return _appVersion; }
            set
            {
                SetProperty(ref _appVersion, value);
            }
        }

        private ObservableCollection<HotWheelsModel> _searchhwResult;
        public ObservableCollection<HotWheelsModel> SearchhwResult
        {
            get { return _searchhwResult; }
            set
            {
                SetProperty(ref _searchhwResult, value);
                RaisePropertyChanged(() => IsPopupHwOpened);
            }
        }

        public ObservableCollection<string> SortOptions { get; private set; } = new ObservableCollection<string>
		{
			"Date Added",
			"A-Z",
			"Z-A"
		};


        private ItemModel? _selectedItem;
        public ItemModel? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

		private object? _currentView = Mvx.IoCProvider.Resolve<HomePageViewModel>();
		public object? CurrentView
		{
			get { return _currentView; }
			set
			{ 
				SetProperty(ref _currentView, value);
                RaisePropertyChanged(() => IsSettingsViewOpen);
			}
		}

        private ObservableCollection<FolderModel> _folders;
        public ObservableCollection<FolderModel> Folders
        {
            get { return _folders; }
            set 
            { 
                SetProperty(ref _folders, value);
                RaisePropertyChanged(() => IsPopupHwOpened);
            }
        }


        private FolderModel? _selectedFolder;
        public FolderModel? SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
				if (_selectedFolder == value) return;
                SetProperty(ref _selectedFolder, value);
                RaisePropertyChanged(() => IsFolderSelected);
                SelectedItem = null;
                if (_selectedFolder != null)
                {
                    NotLoadingItems = false;
                    Task.Run(NavigateAndLoadListCollection);
                }
                SelectedSortOption = SortOptions[0];
            }
        }

        private string _selectedSortOption;
		public string SelectedSortOption
		{
			get { return _selectedSortOption; }
			set
			{
				SetProperty(ref _selectedSortOption, value);
                UpdateFolderItemsMessage message = new UpdateFolderItemsMessage(this, SelectedSortOption, "SortItems");
                _messenger.Publish(message);
			}
		}

        private string _searchhwText = " Add HotWheels...";
		public string SearchhwText
        {
			get { return _searchhwText; }
			set 
			{ 
				SetProperty(ref _searchhwText, value);
				_debounceTimer.Stop();
				_debounceTimer.Start();
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
                _debounceTimerSearchBox.Stop();
                _debounceTimerSearchBox.Start();
            }
		}

        private bool _isLoadingItems;
        public bool IsLoadingItems
        {
            get { return _isLoadingItems; }
            set
            { 
                SetProperty(ref _isLoadingItems, value);
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

        private bool _notLoadingItems = true;
        public bool NotLoadingItems
        {
            get { return _notLoadingItems; }
            set
            {
                SetProperty(ref _notLoadingItems, value);
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


		/// <summary>
        /// ========== CARS STATS PROPERTIES
        /// </summary>
		private int _totalHotWheelsCount = 0;
		public int TotalHotWheelsCount
        {
			get
			{ return _totalHotWheelsCount; }
			set 
			{
                SetProperty(ref _totalHotWheelsCount, value);
			}
		}

		private int _totalCarsCount;
		public int TotalCarsCount
        {
			get { return _totalCarsCount; }
			set 
			{
				SetProperty(ref _totalCarsCount, value);
			}
		}

		private int _totalFolderCarsCount;
		public int TotalFolderCarsCount
        {
			get { return _totalFolderCarsCount; }
			set 
			{ 
				SetProperty(ref _totalFolderCarsCount, value);
			}
		}

        /// <sumamry>
        /// ======== ON CLOSING VIEWMODEL FUNCTIONALITIES
        /// <summary>
        public override void ViewDisappeared()
        {
            UnsubscribeMessages();
            base.ViewDisappeared();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            UnsubscribeMessages();
            base.ViewDestroy(viewFinishing);
        }
    }
}
