
using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.UserReviews
{
    [Serializable, DataContract]
    public class PopularBikesWithUserReviews : BasicBikeEntityBase
    {
        [DataMember]
        public uint ReviewCount { get; set; }
        [DataMember]
        public uint RatingsCount { get; set; }
        [DataMember]
        public float OverallRating { get; set; }
    }
}
