using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ItemsProject.Core.Messages.HwListVm_Messages;
using ItemsProject.Wpf.Helper_Functions;
using MaterialDesignThemes.Wpf;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;

namespace ItemsProject.Wpf.Views
{
    /// <summary>
    /// Interaction logic for HwListCollectionView.xaml
    /// </summary>
    public partial class HwListCollectionView : MvxWpfView
    {
        private readonly IMvxMessenger? _messenger;
        private readonly List<MvxSubscriptionToken?> _tokens = new List<MvxSubscriptionToken?>();

        public HwListCollectionView()
        {
            InitializeComponent();
            _messenger = Mvx.IoCProvider?.Resolve<IMvxMessenger>(); ;

            _tokens.Add(_messenger.Subscribe<ClosePopupBeforeDeletingItemMessage>(OnClosePopupMessage));
        }

        private void OnClosePopupMessage(ClosePopupBeforeDeletingItemMessage message)
        {
            foreach (PopupBox popup in FindChildrenInTemplates.FindVisualChildren<PopupBox>(this))
            {
                if (popup.Name == "popupBoxMenu")
                {
                    popup.IsPopupOpen = false;
                }
            }
        }

        private void comboBoxChooseFolder_DropDownClosed(object sender, EventArgs e)
        {
            foreach (PopupBox popup in FindChildrenInTemplates.FindVisualChildren<PopupBox>(this))
            {
                if (popup.Name == "popupBoxMenu" && popup.IsMouseDirectlyOver)
                {
                    popup.IsPopupOpen = false;
                }
            }
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

        protected override void Dispose(bool disposing)
        {
            foreach (var token in _tokens)
            {
                token.Dispose();
            }
            _tokens.Clear();

            base.Dispose(disposing);
        }
    }
}
