using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 10*-10-2017
    /// Description : To Hold data of top rated bikes widget
    /// </summary>
    public class TopRatedBikesWidgetVM
    {
        public IEnumerable<TopRatedBikes> Bikes { get; set; }
        public string WidgetHeading { get; set; }
    }
}
