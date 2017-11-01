using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;

namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class PopularBikesWithExpertReviews : BasicBikeEntityBase
    {
        public CityEntityBase City { get; set; }
        public uint ExpertReviewCount { get; set; }
        public uint Price { get; set; }
        public bool IsOnRoadPrice { get; set; }
    }
}
