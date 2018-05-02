using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Shared;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Mar 2017
    /// Summary    : View model for news listing page
    /// Modified by: Snehal Dange on 24 August 2017
    /// Summary    : Added PopularScooterMakesWidget to show popular brands
    /// Modified by sajal Gupta on 01-12-2017
    /// Summary : Added PopularBikesAndPopularScootersWidget and UpcomingBikesAndUpcomingScootersWidget and PopularBikesAndUpcomingBikesWidget
    /// Modifies by : Ashutosh Sharma on 27 Nov 2017
    /// Description : Added Series.
    /// Modified by: Snehal Dange on 21th dec 2017
    /// Summary : added MoreAboutScootersWidgetVM
    /// Modified by : Rajan Chauhan on 27 Dec 2017
    /// Description : Changed PopularSeriesAndMakeBikeSeriesWidget to PopularSeriesAndBodyStyleWidget and 
    ///               Changed name of PopularUpcomingBodyStyleWidgetWidget to PopularUpcomingBodyStyleWidget
    ///               Added attribute UpcomingBikesByBodyStyleWidget
    /// Modified by : Ashutosh Sharma on 24 Feb 2018
    /// Description : All common properties moved to CmsArticlesListIndexPageVM.
    /// </summary>
    public class NewsIndexPageVM : CmsArticlesListIndexPageVM
    {
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets { get; set; }
    }

    public class EditorialSeriesWidgetVM
    {
        public string SeriesName { get; set; }
        public string WidgetLinkTitle { get; set; }
        public string WidgetLink { get; set; }
        public IEnumerable<MostPopularBikesBase> PopularSeriesBikes { get; set; }
        public IEnumerable<BestBikeEntityBase> PopularBikesByBodyStyle { get; set; }
        public IEnumerable<UpcomingBikeEntity> UpcomingBikesByBodyStyle { get; set; }

    }

    public class EditorialSeriesMobileWidgetVM
    {
        public PopularBodyStyleVM PopularSeriesBikesVM { get; set; }
        public BestBikesEditorialWidgetVM PopularBikesByBodyStyleVM { get; set; }
    }

}
