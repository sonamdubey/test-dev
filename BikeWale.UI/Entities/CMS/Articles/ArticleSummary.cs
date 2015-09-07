using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>
    public class ArticleSummary : ArticleBase
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
    }
}
