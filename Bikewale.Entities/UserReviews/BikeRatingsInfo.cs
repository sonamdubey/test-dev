using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.UserReviews
{
    public class BikeRatingsInfo : BasicBikeEntityBase
    {
        public uint TotalReviews { get; set; }
        public uint TotalRatings { get; set; }
        public uint OneStarRatings { get; set; }
        public uint TwoStarRatings { get; set; }
        public uint ThreeStarRatings { get; set; }
        public uint FourStarRatings { get; set; }
        public uint FiveStarRatings { get; set; }
    }
}
