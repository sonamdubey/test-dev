using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Upcoming
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
    }
}
