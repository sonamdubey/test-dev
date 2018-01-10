using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Models.BikeModels;
using System.Collections.Generic;

namespace Bikewale.Models
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
        public MostPopularBikeWidgetVM MostPopularMakeBikes { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }
        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndUpcomingBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeBikesAndBodyStyleBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeScootersAndOtherBrandsWidget { get; set; }
        public MultiTabsWidgetVM PopularScootersAndUpcomingScootersWidget { get; set; }
    }
}
