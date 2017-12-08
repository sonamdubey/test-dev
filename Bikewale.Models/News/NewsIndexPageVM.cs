﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models.BikeModels;
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
    /// </summary>
    public class NewsIndexPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public CMSContent Articles { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string PageH1 { get; set; }
        public string PageH2 { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public MostPopularBikeWidgetVM MostPopularMakeBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public PwaReduxStore ReduxStore { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }
        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndUpcomingBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeBikesAndBodyStyleBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeScootersAndOtherBrandsWidget { get; set; }
        public MultiTabsWidgetVM PopularScootersAndUpcomingScootersWidget { get; set; }
    }
}
