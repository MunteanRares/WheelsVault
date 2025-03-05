using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsProject.Core.Models
{
    public class LoadListCollectionPrepareModel
    {
        public LoadListCollectionPrepareModel(FolderModel selectedFolder, ObservableCollection<FolderModel> folders)
        {
            SelectedFolder = selectedFolder;
            Folders = folders;  
        }

        public FolderModel SelectedFolder { get; set; }
        public ObservableCollection<FolderModel> Folders { get; set; }
    }
}
