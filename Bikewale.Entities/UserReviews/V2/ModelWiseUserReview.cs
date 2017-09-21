using System;
using System.Collections.Generic;

namespace Bikewale.Entities.UserReviews.V2
{
    /// <summary>
    /// Created By :Snehal Dange on 13th Sep 2017
    /// Description: Created for Reviews tab on Compare Details page
    /// </summary>
    [Serializable]
    public class ModelWiseUserReview
    {
        public uint VersionId { get; set; }
        public uint ModelId { get; set; }
        public uint RatingCount { get; set; }
        public uint ReviewCount { get; set; }
        public float ReviewRate { get; set; }
        public uint Mileage { get; set; }
        public IEnumerable<QuestionRatingsValueEntity> Questions { get; set; }
        public UserReviewSummary UserReviews { get; set; }
    }
}
