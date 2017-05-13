using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class BikeRatingsInfo : BasicBikeEntityBase
    {
        public float OverallRating { get; set; }
        public uint TotalReviews { get; set; }
        public uint TotalRatings { get; set; }
        public uint OneStarRatings { get; set; }
        public uint TwoStarRatings { get; set; }
        public uint ThreeStarRatings { get; set; }
        public uint FourStarRatings { get; set; }
        public uint FiveStarRatings { get; set; }
    }
}
