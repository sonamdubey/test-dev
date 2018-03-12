using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models.BikeModels;
using System.Web;

namespace Bikewale.Models.Shared
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 24 Feb 2018.
    /// Description : Moved common properties of NewsIndexPageVM and ExpertReviewsIndexPageVM here.
    /// </summary>
    public class CmsArticlesListIndexPageVM : ModelBase
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
        public MoreAboutScootersWidgetVM ObjMoreAboutScooter { get; set; }

        public MultiTabsWidgetVM PopularSeriesAndBodyStyleWidget { get; set; }
        public MultiTabsWidgetVM PopularUpcomingBodyStyleWidget { get; set; }

        public EditorialSeriesWidgetVM SeriesWidget { get; set; }
        public EditorialSeriesMobileWidgetVM SeriesMobileWidget { get; set; }
        public MultiTabsWidgetVM SeriesBikesAndModelBodyStyleBikes { get; set; }
        public MultiTabsWidgetVM SeriesBikesAndOtherBrands { get; set; }
        public PwaReduxStore ReduxStore { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }
    }
}
