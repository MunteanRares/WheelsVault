using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.DataAccess.Wizard.Services;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Messages.HwListVm_Messages;
using ItemsProject.Wpf.Helper_Functions;
using MaterialDesignThemes.Wpf;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;


namespace ItemsProject.Wpf.Views
{
    public partial class BaseView : MvxWpfView
    {
        private readonly IMvxMessenger? _messenger;
        private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();
        protected bool isDropDownOpened = false;

        public BaseView()
        {
            Window window = Application.Current.MainWindow;
            InitializeComponent();
            window.Show();
            window.ShowInTaskbar = true;
            _messenger = Mvx.IoCProvider?.Resolve<IMvxMessenger>();
            _tokens.Add(_messenger.Subscribe<CancelItemEditingMessage>(OnCancelItemEditingMessage));
        }

        private void EditFolderNameTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox editTextBox = (TextBox)sender;
            Keyboard.Focus(editTextBox);
            editTextBox.CaretIndex = editTextBox.Text.Length;
        }

        //private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("[^0-9]+");
        //    e.Handled = regex.IsMatch(e.Text);
        //}

        private void OnCancelItemEditingMessage(CancelItemEditingMessage cancelItemEditingMessage)
        {
            bool isMouseOverEditingPanel = false;

            foreach (StackPanel stackPanel in FindChildrenInTemplates.FindVisualChildren<StackPanel>(this))
            {
                if (stackPanel.Name == "stackPanelItemEdit")
                {
                    if (stackPanel.IsMouseOver)
                    {
                        isMouseOverEditingPanel = true;
                    }
                }
            }

            if (!isMouseOverEditingPanel)
            {
                mainGrid.Focus();
                var cancelItemEditing = (ICommand)DataContext.GetType().GetProperty("CancelItemEditCommand")?.GetValue(DataContext);
                var selectedItem = DataContext.GetType().GetProperty("SelectedItem").GetValue(DataContext);
                cancelItemEditing?.Execute(selectedItem);
            }
        }

        public CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point Offset)
        {
            Point placementPoint = new Point(-popupSize.Width, -popupSize.Height + 35);

            return
            [
                new CustomPopupPlacement(placementPoint, PopupPrimaryAxis.Vertical)
            ];
        }

        /// <summary>
        /// TEXTBOX SEARCHHWTEXTBOX HANDLERS
        /// </summary>
        private void hotwheelsAddTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (hotwheelsAddTextBox.Text == " Add HotWheels...")
            {
                hotwheelsAddTextBox.Text = "";
                hotwheelsAddTextBox.Foreground = Brushes.Black;
            }
        }

        private void hotwheelsAddTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            hotwheelsAddTextBox.Clear();

            if (string.IsNullOrEmpty(hotwheelsAddTextBox.Text))
            {
                hotwheelsAddTextBox.Text = " Add HotWheels...";
                hotwheelsAddTextBox.Foreground = Brushes.Gray;
            }
        }

        private void hotwheelsAddTextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (hotWheelsPopup.IsMouseOver)
            {
                e.Handled = true;
            }
        }

        private void hotwheelsAddTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!hotWheelsPopup.IsMouseOver)
            {
                mainGrid.Focus();
            }
        }
    }
}
