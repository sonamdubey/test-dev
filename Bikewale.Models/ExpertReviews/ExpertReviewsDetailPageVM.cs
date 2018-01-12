using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Models.BikeModels;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Mar 2017
    /// Summary    : View model for expert reviews detail page
    /// Modified by sajal Gupta on 24-08-2017
    /// description : added PopularScooterMakesWidget
    /// </summary>
    public class ExpertReviewsDetailPageVM : ModelBase
    {
        public ArticlePageDetails ArticleDetails { get; set; }
        public EditCMSPhotoGalleryVM PhotoGallery { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM SeriesBikes { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public StringBuilder BikeTested { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }
        public IEnumerable<VehicleTag> TaggedBikes { get; set; }
        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }
        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndUpcomingBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeBikesAndBodyStyleBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeScootersAndOtherBrandsWidget { get; set; }
        public MultiTabsWidgetVM PopularScootersAndUpcomingScootersWidget { get; set; }
        public MostPopularBikeWidgetVM MostPopularMakeBikes { get; set; }
        public SimilarBikesWidgetVM SimilarBikes { get; set; }
        public bool IsSeriesAvailable { get; set; }
        public bool IsScooter { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public MultiTabsWidgetVM SeriesBikesAndOtherBrands { get; set; }
        public MultiTabsWidgetVM SeriesBikesAndModelBodyStyleBikes { get; set; }
        public EditorialSeriesWidgetVM SeriesWidget { get; set; }
    }
}
