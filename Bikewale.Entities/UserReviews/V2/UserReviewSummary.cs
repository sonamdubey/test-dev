using System;

namespace Bikewale.Entities.UserReviews.V2
{
    /// <summary>
    /// Modified by Snehal Dange on 11 Sep 2017
    /// Description: Added 3 para :ReviewRate ,RatingsCount, ReviewCount
    /// </summary>
    [Serializable]
    public class UserReviewSummary
    {
        public uint ReviewId { get; set; }
        public ushort OverallRatingId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }        
    }
}
