using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ItemsProject.Wpf.Helper_Functions;
using MvvmCross.Platforms.Wpf.Views;


namespace ItemsProject.Wpf.Views
{
    public partial class BaseView : MvxWpfView
    {
        public BaseView()
        {
            InitializeComponent();
        }

        protected bool isDropDownOpened = false;

        private void comboBoxSort_DropDownOpened(object sender, EventArgs e)
        {
            isDropDownOpened = true;
        }

        private void comboBoxSort_DropDownClosed(object sender, EventArgs e)
        {
            isDropDownOpened = false;
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

        private void MvxWpfView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool isMouseOverEditingPanel = false;
            bool isMouseOverComboBox = false;

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

            if (isDropDownOpened)
            {
                isMouseOverComboBox = true;
            }

            if (!isMouseOverEditingPanel && !isMouseOverComboBox)
            {
                mainGrid.Focus();
                var cancelItemEditing = (ICommand)DataContext.GetType().GetProperty("CancelItemEditCommand")?.GetValue(DataContext);
                var selectedItem = DataContext.GetType().GetProperty("SelectedItem").GetValue(DataContext);
                cancelItemEditing?.Execute(selectedItem);
            }
        }
    }
}
