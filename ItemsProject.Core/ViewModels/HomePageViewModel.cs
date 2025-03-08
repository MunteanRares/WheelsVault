
using System.Collections.ObjectModel;
using System.Windows.Input;
using Timer = System.Timers.Timer;
using ItemsProject.Core.Commands.HomePageCommands;
using ItemsProject.Core.Models;
using ItemsProject.Core.Services;
using MvvmCross.Base;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.ViewModels
{
    public class HomePageViewModel : MvxViewModel
    {
        private readonly IHomePageService _homePageService;
        private readonly IMvxMainThreadAsyncDispatcher _mvxThread;
        private readonly IDataService _dataService;

        private int _currentIndex = 0;
        private Timer _debounceTimer;
        private Timer _countDownTimer;

        public HomePageViewModel(IMvxMessenger messenger, IHomePageService homePageService, IMvxMainThreadAsyncDispatcher mvxThread, IDataService dataService)
        {
            _homePageService = homePageService;
            _mvxThread = mvxThread;
            _dataService = dataService;

            // COMMANDS 
            SelectDefaultFolderCommand = new SelectDefaultFolderCommand(messenger);
            AddCurrentItemToCollectionCommand = new AddCurrentItemToCollectionCommand(messenger, () => CurrentDisplayedItem, _dataService, UpdateCurrentItemCollectionAsync);

            //TIMERS
            _countDownTimer = new Timer(70);
            _countDownTimer.Elapsed += _countDownTimer_Elapsed;

            _debounceTimer = new Timer(7000);
            _debounceTimer.Elapsed += _debounceTimer_Elapsed;
            _debounceTimer.Start();
        }

        private void _countDownTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (CountDownValue > 0)
            {
                CountDownValue--;
            }
            else
            {
                _countDownTimer?.Stop();
            }
        }

        private async void _debounceTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            StartCountDown();

            if (_currentIndex <= LatestCars.Count() - 1)
            {
                _currentIndex += 1;
                CurrentDisplayedItem = LatestCars[_currentIndex];
            }
            else
            {
                _currentIndex = 0;
                CurrentDisplayedItem = LatestCars[_currentIndex];
                return;
            }

            await UpdateCurrentItemCollectionAsync(CurrentDisplayedItem);
        }

        public void StartCountDown()
        {
            CountDownValue = 100;
            _countDownTimer.Start();
        }

        /// <summary>
        /// ========= COMMANDS
        /// </summary>
        public ICommand SelectDefaultFolderCommand { get; }
        public ICommand AddCurrentItemToCollectionCommand { get; }


        /// <summary>
        /// ========== PROPERTIES
        /// </summary>
        private ObservableCollection<ItemModel> _latestCars;
        public ObservableCollection<ItemModel> LatestCars
        {
            get { return _latestCars; }
            set 
            { 
                SetProperty(ref _latestCars, value);
                CurrentDisplayedItem = LatestCars[_currentIndex];
                StartCountDown();
                _ = UpdateCurrentItemCollectionAsync(CurrentDisplayedItem);
            }
        }

        private ItemModel _currentDisplayedItem;
        public ItemModel CurrentDisplayedItem
        {
            get { return _currentDisplayedItem; }
            set 
            { 
                SetProperty(ref _currentDisplayedItem, value);  
            }
        }

        public async Task UpdateCurrentItemCollectionAsync(ItemModel itemModel)
        {
            IsCurrentItemInCollection = await _homePageService.IsCurrentItemInCollection(itemModel);
        }

        private bool _isCurrentItemInCollection;
        public bool IsCurrentItemInCollection
        {
            get { return _isCurrentItemInCollection; }
            set 
            { 
                SetProperty(ref _isCurrentItemInCollection, value);
            }
        }


        private int _countDownValue;
        public int CountDownValue
        {
            get { return _countDownValue; }
            set
            {
                SetProperty(ref _countDownValue, value);
            }
        }


        public async override Task Initialize()
        {
            await AsyncDispatcher.ExecuteOnMainThreadAsync(async () => LatestCars = new ObservableCollection<ItemModel>(await _homePageService.GetLatestCars()));
            await base.Initialize();
        }

    }
}
