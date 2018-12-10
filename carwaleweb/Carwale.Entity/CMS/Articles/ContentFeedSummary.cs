using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    public class ContentFeedSummary
    {
        public ulong BasicId { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string DisplayDate { get; set; }
        public ulong Views { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public uint MainImageSet { get; set; }
        public bool IsMainImage { get; set; }
        public string ImagePathThumbnail { get; set; }
        public string ImagePathLarge { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostUrl { get; set; }
        public string Content { get; set; }
    }
}
