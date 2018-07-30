using System;

namespace Bikewale.DTO.CMS.Articles
{
    public class CMSArticleSummary : CMSArticleBase
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
    }
}
