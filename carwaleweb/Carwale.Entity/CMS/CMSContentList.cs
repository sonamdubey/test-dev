using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    /// <summary>
    /// Holds information for the list of items on the home page.
    /// </summary>
    /// 

    [Serializable]
    public class CMSContentList
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string DisplayDate { get; set; }
        public string Views { get; set; }
        public string NewsUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public string ThumbNailImageUrl { get; set; }
        public bool IsSticky { get; set; }
        public string CommentCount { get; set; }
        public int RowNumber { get; set; }
        public string SmallImageUrl { get; set; }
        public string AuthorMaskingName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public int CategoryId { get; set; }
        public string MakeName { get; set; }
        public string ModelMaskingName { get;set;}
    }
}
