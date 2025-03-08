using ItemsProject.Core.Models;

namespace ItemsProject.Core.Commands.General
{
    public class CancelItemEdit : CommandBase
    {
        private readonly Action<ItemModel> _stopItemEditing;

        public CancelItemEdit(Action<ItemModel> stopItemEditing)
        {
            _stopItemEditing = stopItemEditing;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                _stopItemEditing((ItemModel)parameter);
            }
        }
    }
}
