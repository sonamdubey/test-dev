using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class BikeReviewsInfo //: BasicBikeEntityBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }

        public uint TotalReviews { get; set; }
        public uint MostHelpfulReviews { get; set; }
        public uint MostRecentReviews { get; set; }
        public uint PostiveReviews { get; set; }
        public uint NegativeReviews { get; set; }
        public uint NeutralReviews { get; set; }

    }
}
