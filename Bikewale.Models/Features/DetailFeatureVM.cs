
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
namespace Bikewale.Models.Features
{
    public class DetailFeatureVM : ModelBase
    {
        public ArticlePageDetails objFeature { get; set; }
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public EditCMSPhotoGalleryVM PhotoGallery { get; set; }
    }
}
