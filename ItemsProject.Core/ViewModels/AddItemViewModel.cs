using ItemsProject.Core.Data;
using ItemsProject.Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using ItemsProject.Core.Messages;

namespace ItemsProject.Core.ViewModels
{
    public class AddItemViewModel : MvxViewModel<FolderModel>
    {
        private readonly IDatabaseData _db;
        private readonly IMvxNavigationService _navigation;
        private readonly IMvxMessenger _messenger;

        public AddItemViewModel(IDatabaseData db, IMvxNavigationService navigation, IMvxMessenger messenger)
        {
            _db = db;
            _navigation = navigation;
            _messenger = messenger;
            cancelCommand = new MvxCommand(Cancel);
            addItemCommand = new MvxCommand(AddItem);
        }

        // Recieving params from other ViewModels
        public override void Prepare(FolderModel parameter)
        {
            SelectedFolderId = parameter.Id;
        }

        // Commands Declaration
        public IMvxCommand cancelCommand { get; set; }
        public IMvxCommand addItemCommand { get; set; }

        // Commands Functionalities
        public void Cancel()
        {
            _navigation.Close(this);
        }

        public void AddItem()
        {
            ItemModel newItem = _db.CreateNewItem(SelectedFolderId, ModelName, ModelReleaseDate, CollectionName);
            var message = new AddedItemMessage(
                this,
                newItem
            );

            _messenger.Publish(message);
            _navigation.Close(this);
        }


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
    }
}
