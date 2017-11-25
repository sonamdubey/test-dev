using System;

namespace Bikewale.DTO.CMS.Articles
{
    public class CMSArticleSummaryMin : CMSArticleBase
    {
        public ushort CategoryId { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgUrl { get; set; }
        public string AuthorName { get; set; }
        public DateTime DisplayDate { get; set; }
        public string FormattedDisplayDate { get { return string.Format("{0:MMM dd, yyyy}", DisplayDate); } }
    }
}
