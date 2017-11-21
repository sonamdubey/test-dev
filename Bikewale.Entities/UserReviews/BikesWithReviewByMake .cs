using Bikewale.Entities.Compare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created By:Snehal Dange on 20th Nov 2017
    /// Description: Entity created for user reviews on make page
    /// </summary>
    [Serializable]
    public class BikesWithReviewByMake
    {
        public PopularBikesWithUserReviews BikeModel { get; set; }
        public Bikewale.Entities.UserReviews.V2.UserReviewSummary MostHelpful { get; set; }
        public Bikewale.Entities.UserReviews.V2.UserReviewSummary MostRecent { get; set; }
    }
}
