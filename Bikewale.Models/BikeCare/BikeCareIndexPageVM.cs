using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Models.BikeModels;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 31 Mar 2017
    /// Summary    : View model for Bike care listing page
    /// </summary>
    public class BikeCareIndexPageVM : ModelBase
    {
        public CMSContent Articles { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }

        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }
    }
}
