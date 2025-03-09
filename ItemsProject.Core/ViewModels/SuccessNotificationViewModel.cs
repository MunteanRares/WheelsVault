using Timer = System.Timers.Timer;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using ItemsProject.Core.Models;
using DevExpress.Data.Helpers;
using ItemsProject.Core.Services;
using ItemsProject.Core.Messages.SuccessNotification_Messages;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Core.ViewModels
{
    public class SuccessNotificationViewModel : MvxViewModel<SuccessNotificationMessageModel>
    {
        private readonly IMvxNavigationService _nav;
        private readonly IMvxMessenger _messenger;

        private Timer _countDownTimer;
        private double _timeElapsed = 0;
        private const double TotalTime = 4000;
        private object? _source;

        public SuccessNotificationViewModel(IMvxNavigationService nav,
                                            IMvxMessenger messenger)
        {
            _nav = nav;
            _messenger = messenger;

            _countDownTimer = new Timer(20);
            _countDownTimer.Elapsed += _countDownTimer_Elapsed;
            _countDownTimer.AutoReset = true;
            _countDownTimer.Start();
        }

        private void _countDownTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _timeElapsed += 20;
            CountDownNotification = 100 - (_timeElapsed / TotalTime * 100);

            if (_timeElapsed > TotalTime)
            {
                _countDownTimer.Stop();
                _nav.Close(this);

                if(_source is SettingsService)
                {
                    EnableButtonMessage message = new EnableButtonMessage(this);
                    _messenger.Publish(message);
                }
            }
        }

        /// <summary>
        /// ========= FUNCTIONS
        /// </summary>
        public override void Prepare(SuccessNotificationMessageModel parameter)
        {
            SuccessMessage = parameter.SuccessMessage;
            _source = parameter.Source;
        }

        /// <summary>
        /// ========= PROPERTIES
        /// </summary>
        private string _successMessage;
        public string SuccessMessage
        {
            get { return _successMessage; }
            set 
            {
                SetProperty(ref _successMessage, value);
            }
        }

        private double _countDownNotification;
        public double CountDownNotification
        {
            get { return _countDownNotification; }
            set 
            { 
               SetProperty(ref _countDownNotification, value);
            }
        }
    }
}
