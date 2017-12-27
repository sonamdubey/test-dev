using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pager;
using Bikewale.Models.BikeModels;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 21 Mar 2017
    /// Summary : Class have properties to render the expert reviews index page (View Model)
    /// Modified by : Aditi Srivastava on 30 Mar 2017
    /// Summary     : Added view models and properties for widgets
    /// Modified by : Vivek Singh Tomar on 27th Nov 2017
    /// Summary : Added BikeSeriesEntityBase
    /// Modified by sajal Gupta on 01-12-2017
    /// Summary : Added PopularBikesAndPopularScootersWidget and UpcomingBikesAndUpcomingScootersWidget and MostPopularMakeBikes
    /// </summary>
    public class ExpertReviewsIndexPageVM : ModelBase
    {
        public CMSContent Articles { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeSeriesEntityBase Series { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string PageH1 { get; set; }
        public string PageH2 { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikesByBodyStyleWidget { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public EditorialPageType EditorialPageType { get; set; }
        public MostPopularBikeWidgetVM MostPopularMakeBikes { get; set; }
        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }
        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndUpcomingBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeBikesAndBodyStyleBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeScootersAndOtherBrandsWidget { get; set; }
        public MultiTabsWidgetVM PopularScootersAndUpcomingScootersWidget { get; set; }

        public MultiTabsWidgetVM PopularSeriesAndBodyStyleWidget { get; set; }
        public MultiTabsWidgetVM PopularUpcomingBodyStyleWidget { get; set; }

        public EditorialSeriesWidgetVM SeriesWidget { get; set; }
        public EditorialSeriesMobileWidgetVM SeriesMobileWidget { get; set; }
    }

}
