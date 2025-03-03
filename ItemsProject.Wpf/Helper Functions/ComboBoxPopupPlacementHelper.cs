using System.Windows;
using System.Windows.Controls.Primitives;


namespace ItemsProject.Wpf.Helper_Functions
{
    public static class ComboBoxPopupPlacementHelper
    {
        public static CustomPopupPlacementCallback LeftPopupPlacementCallback => new CustomPopupPlacementCallback((popupSize, targetSize, offset) =>
        {
            // For instance, position the popup to the left.
            CustomPopupPlacement placement = new CustomPopupPlacement(new Point(targetSize.Width - (popupSize.Width - 15) * 2, targetSize.Height - 35), PopupPrimaryAxis.Vertical);
            return new CustomPopupPlacement[] { placement };
        });
    }
}
