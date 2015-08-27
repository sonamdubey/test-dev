using Bikewale.Entity.CMS.Articles;
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
    [Serializable]
    public class ArticlePageDetails : ArticleSummary
    {
        public List<Page> PageList { get; set; }
        public List<string> TagsList { get; set; }
        public List<VehicleTag> VehiclTagsList { get; set; }

        public ArticleBase NextArticle { get; set; }
        public ArticleBase PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
    }
}
