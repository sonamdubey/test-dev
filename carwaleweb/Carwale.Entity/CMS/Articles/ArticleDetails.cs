using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    [Serializable]
    public class ArticleDetails : ArticleSummary
    {
        public string Content { get; set; }
        public List<string> TagsList { get; set; }
        public List<VehicleTag> VehiclTagsList { get; set; }

        public ArticleSummary NextArticle { get; set; }
        public ArticleSummary PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
    }
}
