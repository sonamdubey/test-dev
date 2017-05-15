
using System;
namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class BikeRatingsReviewsInfo
    {
        public BikeReviewsInfo ReviewDetails { get; set; }
        public BikeRatingsInfo RatingDetails { get; set; }
        public uint Price { get; set; }
    }
}
