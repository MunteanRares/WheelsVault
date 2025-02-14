using ItemsProject.Core.Commands.General;
using ItemsProject.Core.Models;


namespace ItemsProject.Core.Commands.BaseViewModelCommands
{
    public class EditItemFromFolder : CommandBase
    {
        private readonly Action<ItemModel, bool> _changeEditMode;
        public EditItemFromFolder(Action<ItemModel, bool> changeEditMode)
        {
            _changeEditMode = changeEditMode;
        }

        public override void Execute(object? parameter)
        {
            ItemModel itemModel = (ItemModel)parameter;
            _changeEditMode(itemModel, !itemModel.IsEditing);
        }
    }
}
