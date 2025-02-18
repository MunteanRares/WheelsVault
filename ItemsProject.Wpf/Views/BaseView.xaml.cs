using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DevExpress.Data.Async.Helpers;
using ItemsProject.Core.Messages;
using ItemsProject.Core.ViewModels;
using ItemsProject.Wpf.Helper_Functions;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;


namespace ItemsProject.Wpf.Views
{
    public partial class BaseView : MvxWpfView
    {
        private readonly IMvxMessenger _messenger;
        private readonly List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();
        protected bool isDropDownOpened = false;

        public BaseView()
        {
            InitializeComponent();
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _tokens.Add(_messenger.Subscribe<CancelItemEditingMessage>(OnCancelItemEditingMessage));
        }

        private void EditFolderNameTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox editTextBox = (TextBox)sender;
            Keyboard.Focus(editTextBox);
            editTextBox.CaretIndex = editTextBox.Text.Length;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

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
    }
}
