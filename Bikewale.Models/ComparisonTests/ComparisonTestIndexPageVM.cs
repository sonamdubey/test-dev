using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Models.BikeModels;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 8 May 2017
    /// Summary :   View model for comparison test landing page
    /// </summary>
    public class ComparisonTestsIndexPageVM : ModelBase
    {
        public CMSContent Articles { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string PageH1 { get; set; }
        public string PageH2 { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }
        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }

    }
}
