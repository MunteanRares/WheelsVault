using DevExpress.XtraExport;
using MvvmCross.ViewModels;

namespace ItemsProject.Core.Models
{
    public class ItemModel : MvxNotifyPropertyChanged
    {
        public int Id { get; set; }

        private List<int> _folderIds = new List<int>();

        public List<int> FolderIds
        {
            get { return _folderIds; }
            set 
            { 
                SetProperty(ref _folderIds, value); 
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

        private string _yearProduced;
        public string YearProduced
        {
            get { return _yearProduced; }
            set
            {
                SetProperty(ref _yearProduced, value);
            }
        }

        private string _yearProducedNum;
        public string YearProducedNum
        {
            get { return _yearProducedNum; }
            set 
            {
                SetProperty(ref _yearProducedNum, value);
            }
        }

        private string _seriesName;
        public string SeriesName
        {
            get { return _seriesName; }
            set
            {
                SetProperty(ref _seriesName, value);
            }
        }

        private string _seriesNum;
        public string SeriesNum
        {
            get { return _seriesNum; }
            set 
            {
                SetProperty(ref _seriesNum, value);
            }
        }

        private string _toyNum;
        public string ToyNum
        {
            get { return _toyNum; }
            set
            {
                SetProperty(ref _toyNum, value);
            }
        }

        private string _photoURL;
        public string PhotoURL
        {
            get { return _photoURL; }
            set
            {
                SetProperty(ref _photoURL, value);
            }
        }

        private bool _isCustom;
        public bool IsCustom
        {
            get { return _isCustom; }
            set
            { 
                SetProperty(ref _isCustom, value);
            }
        }


        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                SetProperty(ref _isEditing, value);
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set 
            { 
                SetProperty(ref _quantity, value);
                RaisePropertyChanged(() => Quantity);
            }
        }


        private bool _isPopupOpened;
        public bool IsPopupOpened
        {
            get { return _isPopupOpened; }
            set 
            { 
                SetProperty(ref _isPopupOpened, value);
            }
        }
    }
}
