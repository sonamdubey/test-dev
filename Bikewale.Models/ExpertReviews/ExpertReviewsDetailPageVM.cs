using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Shared;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Mar 2017
    /// Summary    : View model for expert reviews detail page
    /// Modified by sajal Gupta on 24-08-2017
    /// description : added PopularScooterMakesWidget
    /// Modified by : Rajan Chauhan on 26 Feb 2018
    /// description : Added base class CMSArticleDetailPageVM
    /// </summary>
    public class ExpertReviewsDetailPageVM : CMSArticleDetailPageVM
    {
        public ArticlePageDetails ArticleDetails { get; set; }
        public EditCMSPhotoGalleryVM PhotoGallery { get; set; }
        public StringBuilder BikeTested { get; set; }
        public IEnumerable<VehicleTag> TaggedBikes { get; set; }
    }
}
