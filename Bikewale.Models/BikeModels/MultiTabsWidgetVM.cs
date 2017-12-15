using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;
namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Created by sajal Gupta on 05*-12-2017
    /// Multi Tabbs widget vm
    /// </summary>
    public class MultiTabsWidgetVM
    {
        public string TabHeading1 { get; set; }
        public string TabHeading2 { get; set; }
        public string ViewPath1 { get; set; }
        public string ViewPath2 { get; set; }
        public string TabId1 { get; set; }
        public string TabId2 { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public MostPopularBikeWidgetVM MostPopularMakeBikes { get; set; }
        public MostPopularBikeWidgetVM MostPopularScooters { get; set; }
        public MostPopularBikeWidgetVM MostPopularMakeScooters { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingScooters { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakes { get; set; }

        public IEnumerable<MostPopularBikesBase> PopularSeriesBikes { get; set; }
        public IEnumerable<MostPopularBikesBase> PopularMakeSeriesBikes { get; set; }
        public IEnumerable<BestBikeEntityBase> PopularBikesByBodyStyle { get; set; }
        public IEnumerable<UpcomingBikeEntity> UpcomingBikesByBodyStyle { get; set; }

        public string ViewAllHref1 { get; set; }
        public string ViewAllHref2 { get; set; }
        public string ViewAllTitle1 { get; set; }
        public string ViewAllTitle2 { get; set; }
        public string ViewAllText1 { get; set; }
        public string ViewAllText2 { get; set; }
        public bool ShowViewAllLink1 { get; set; }
        public bool ShowViewAllLink2 { get; set; }
        public MultiTabWidgetPagesEnum Pages { get; set; }
        public string PageName { get; set; }
    }

    public enum MultiTabWidgetPagesEnum
    {
        PopularBikesAndPopularScooters = 1,
        UpcomingBikesAndUpcomingScooters = 2,
        PopularBikesAndUpcomingBikes = 3,
        PopularMakeBikesAndBodyStyleWidget = 4,
        PopularMakeScootersAndOtherBrands = 5,
        PopularScootersAndUpcomingScooters = 6,
        PopularSeriesAndMakeBikeSeriesWidget = 7,
        PopularUpcomingBodyStyleWidgetWidget = 8
    }
}
