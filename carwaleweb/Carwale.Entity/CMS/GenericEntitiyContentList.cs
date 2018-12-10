using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    [Serializable]
    public class GenericEntitiyContentList
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ContentId { get; set; }
        public string Url { get; set; }
        public int Views { get; set; }
        public string AuthorName { get; set; }
        public DateTime DisplayDate { get; set; }
        public bool IsMainImage { get; set; }
        public string HostURL { get; set; }
        public string ImagePathThumbNail { get; set; }
        public string ImagePathLarge { get; set; }
        public string Tag { get; set; }
        public string Caption { get; set; }
        public string MainImgCaption { get; set; }
        public int ImageId { get; set; }
        public bool IsPublished { get; set; }
    }
}
