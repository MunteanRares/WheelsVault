using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsProject.Core.Models
{
    public class MessageBoxModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string IconSource { get; set; }
        public FolderModel FolderToDelete { get; set; }
    }
}
