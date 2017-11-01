using System;

namespace Bikewale.Sitemap.MainSitemap
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Oct 2017
    /// Description :   Edit CMS article entity
    /// </summary>
    public class ArticleEntity
    {
        public ushort CategoryId { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }

        public string Description { get; set; }
        public string AuthorName { get; set; }
        public DateTime DisplayDate { get; set; }
        public uint Views { get; set; }
        public bool IsSticky { get; set; }
        public uint FacebookCommentCount { get; set; }
        public string OriginalImgUrl { get; set; }

        public string FormattedDisplayDate { get; set; }
        public string ShareUrl { get; set; }
        public bool IsFeatured { get; set; }

        public ulong BasicId { get; set; }
        public string Title { get; set; }
        public string ArticleUrl { get; set; }
    }
}
