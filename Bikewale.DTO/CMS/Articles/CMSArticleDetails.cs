using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>
    [Serializable]
    public class CMSArticleDetails : CMSArticleSummary
    {
        public string Content { get; set; }
        public List<string> TagsList { get; set; }
        public List<CMSVehicleTag> VehiclTagsList { get; set; }

        public CMSArticleBase NextArticle { get; set; }
        public CMSArticleBase PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
        public string AuthorMaskingName { get; set; }
    }
}
