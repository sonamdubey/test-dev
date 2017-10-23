using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;

namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class PopularBikesWithExpertReviews
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public CityEntityBase City { get; set; }
        public uint ExpertReviewCount { get; set; }
        public uint Price { get; set; }
        public bool IsOnRoadPrice { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
    }
}
