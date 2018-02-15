using System;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created By:Snehal Dange on 20th Nov 2017
    /// Description: Entity created for user reviews on make page
    /// Modified by : Snehal Dange on 5th Feb 2018
    /// Decsription : Added ModelCountWithUserReviews,MakeReviewCount 
    /// </summary>
    [Serializable]
    public class BikesWithReviewByMake
    {
        public PopularBikesWithUserReviews BikeModel { get; set; }
        public Bikewale.Entities.UserReviews.V2.UserReviewSummary MostHelpful { get; set; }
        public Bikewale.Entities.UserReviews.V2.UserReviewSummary MostRecent { get; set; }
        public uint ModelCountWithUserReviews { get; set; }
        public uint MakeReviewCount { get; set; }
    }
}
