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
    public class ArticlePageDetails : ArticleSummary
    {
        public List<Page> PageList { get; set; }
        public List<string> TagsList { get; set; }
        public List<VehicleTag> VehiclTagsList { get; set; }

        public ArticleBase NextArticle { get; set; }
        public ArticleBase PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
        public bool ShowGallery { get; set; }

        public string CategoryName { get; set; }//added by sachin bharti on 15/9/15

        public string SmallDescription { get; set; }
    }
}
