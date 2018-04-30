using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
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

        public System.Collections.Generic.IDictionary<Entities.BikeData.EditorialPageWidgetPosition, Shared.EditorialWidgetVM> PageWidgets { get; set; }
    }
}
