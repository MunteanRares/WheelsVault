using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsProject.Core.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string ModelName { get; set; }
        public string ModelReleaseDate { get; set; }
        public string CollectionName { get; set; }
    }
}
