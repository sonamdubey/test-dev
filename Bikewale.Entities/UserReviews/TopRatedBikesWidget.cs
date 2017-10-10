using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;
using System.Runtime.Serialization;


namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 10-10-2017 to hold data of top rated bikes
    /// </summary>
    [Serializable, DataContract]
    public class TopRatedBikes
    {
        [DataMember]
        public BikeMakeEntityBase Make { get; set; }
        [DataMember]
        public BikeModelEntityBase Model { get; set; }
        [DataMember]
        public CityEntityBase City { get; set; }
        [DataMember]
        public uint ReviewCount { get; set; }
        [DataMember]
        public uint RatingsCount { get; set; }
        [DataMember]
        public float ReviewRate { get; set; }
        [DataMember]
        public uint ExShowroomPrice { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
    }
}
