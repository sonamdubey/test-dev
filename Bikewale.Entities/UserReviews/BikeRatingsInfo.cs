using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System;

namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class BikeRatingsInfo //: BasicBikeEntityBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }

        public float OverallRating { get; set; }
        public uint TotalReviews { get; set; }
        public uint TotalRatings { get; set; }
        public uint OneStarRatings { get; set; }
        public uint TwoStarRatings { get; set; }
        public uint ThreeStarRatings { get; set; }
        public uint FourStarRatings { get; set; }
        public uint FiveStarRatings { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public uint ExpertReviewsCount { get; set; }
        public uint MaximumRatings { get { return Math.Max(Math.Max(Math.Max(OneStarRatings, TwoStarRatings), Math.Max(ThreeStarRatings, FourStarRatings)), FiveStarRatings); } } // added by sajal gupta
    }
}
