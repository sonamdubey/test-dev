using System;

namespace Bikewale.DTO.UserReviews
{
    public class BikeRatingData
    {
        public float OverallRating { get; set; }
        public uint TotalReviews { get; set; }
        public uint TotalRatings { get; set; }
        public uint OneStarRatings { get; set; }
        public uint TwoStarRatings { get; set; }
        public uint ThreeStarRatings { get; set; }
        public uint FourStarRatings { get; set; }
        public uint FiveStarRatings { get; set; }

        public uint MaximumRatings { get { return Math.Max(Math.Max(Math.Max(OneStarRatings, TwoStarRatings), Math.Max(ThreeStarRatings, FourStarRatings)), FiveStarRatings); } }
    }
}