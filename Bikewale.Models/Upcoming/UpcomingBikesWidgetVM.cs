using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 28 Mar 2017
    /// Summary    : View model for upcoming bikes
    /// </summary>
    public class UpcomingBikesWidgetVM
    {
        public IEnumerable<UpcomingBikeEntity> UpcomingBikes { get; set; }
        public string WidgetHeading { get; set; }
        public string WidgetHref { get; set; }
        public string WidgetLinkTitle { get; set; }
        public bool ShowViewAllLink { get; set; }
    }
}
