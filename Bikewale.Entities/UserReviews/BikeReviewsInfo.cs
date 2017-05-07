using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.UserReviews
{
    public class BikeReviewsInfo : BasicBikeEntityBase
    {
        public uint TotalReviews { get; set; }
        public uint MostHelpfulReviews { get; set; }
        public uint MostRecentReviews { get; set; }
        public uint PostiveReviews { get; set; }
        public uint NegativeReviews { get; set; }
        public uint NeutralReviews { get; set; }
    }
}
