using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using System.Text;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Mar 2017
    /// Summary    : View model for expert reviews detail page
    /// </summary>
    public class ExpertReviewsDetailPageVM : ModelBase
    {
        public ArticlePageDetails ArticleDetails { get; set; }
        public EditCMSPhotoGalleryVM PhotoGallery { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public StringBuilder BikeTested { get; set; }
    }
}
