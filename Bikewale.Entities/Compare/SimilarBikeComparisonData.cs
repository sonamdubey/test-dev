using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    [Serializable, DataContract]
    public class SimilarBikeComparisonData
    {
        [DataMember]
        public BikeMakeBase BikeMake { get; set; }
        [DataMember]
        public BikeModelEntityBase BikeModel { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public uint ModelId1 { get; set;}
        public uint ModelId2 { get; set; }
    }
}
