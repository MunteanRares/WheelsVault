using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemsProject.Core.Commands.BaseViewModelCommands;
using ItemsProject.Core.Commands.BaseViewModelCommands.HotWheels_Commands;
using ItemsProject.Core.Commands.BaseViewModelCommands.Item_Commands;
using ItemsProject.Core.Commands.BaseViewModelCommands.Opening_Commands;
using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Helper_Methods.String_Manipulation;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
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

        public HwListCollectionViewModel(IDataService dataService, IMvxMessenger messenger)
        {
            _dataService = dataService;
            _messenger = messenger;

            _tokens.Add(_messenger.Subscribe<AddedHwMessage>(OnAddedHwMessage));

            FolderItems = new ObservableCollection<ItemModel>();

            // Opening Commands
            OpenPopupCommand = new OpenPopupCommand(SetSelectedItemFolderIds, SetIsCheckedIfItemInFolder);

            // Item Commands
            CancelItemEditCommand = new CancelItemEdit(CancelItemEditing);
            DeleteItemFromFolderCommand = new DeleteItemFromFolder(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems, () => SelectedFolder);
            DeleteAllItemsCommand = new DeleteAllItemsCommand(_dataService, ExecuteUpdateFolderItems, () => _allFolderItems);
            EditModeItemCommand = new EditItemFromFolder(EditModeItems);
            LoseItemFocusCommand = new CancelItemEditingCommand(_dataService);
            SaveEditItemCommand = new SaveEditItem(_dataService, () => EditingItemName, () => EditingItemReleaseDate, () => EditingItemCollectionName, SaveItemEdit);
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

        private void OnAddedHwMessage(AddedHwMessage message)
        {
            UpdateFolders(message.NewItem);
        }

        public override void Prepare(LoadListCollectionPrepareModel parameter)
        {
            SelectedFolder = parameter.SelectedFolder;
            Folders = parameter.Folders;
            _allFolderItems = _dataService.LoadItemsForFolder(SelectedFolder);
            FolderItems = _dataService.UpdateFolderItems(_allFolderItems, FolderItems);
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

        private void UpdateFolders(ItemModel newItem)
        {
            if (newItem.Quantity == 1)
            {
                _allFolderItems.Add(newItem);
                FolderItems.Add(newItem);
            }

            FolderItems = _dataService.UpdateFolderItems(_dataService.LoadItemsForFolder(SelectedFolder), FolderItems);
            UpdateCarCounts();
        }

        public void ExecuteUpdateFolderItems(List<ItemModel> updatedItems)
        {
            _allFolderItems = updatedItems;
            FolderItems = _dataService.UpdateFolderItems(updatedItems, FolderItems);

            UpdateCarCounts();
        }

        private void UpdateCarCounts()
        {
            TotalHotWheelsCount = _dataService.GetAllHotWheelsCount();
            TotalCarsCount = _dataService.GetAllCarsCount();
            TotalFolderCarsCount = FolderItems.Count();
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

        private int _totalHotWheelsCount;
        public int TotalHotWheelsCount
        {
            get { return _totalHotWheelsCount; }
            set
            { 
               SetProperty(ref  _totalHotWheelsCount, value);   
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
    }
}
