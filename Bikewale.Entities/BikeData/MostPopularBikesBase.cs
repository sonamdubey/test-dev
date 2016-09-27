using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By  : Sushil Kumar
    /// Description : Bike Entity to show most popular bikes by make
    /// Modified by : Sajal Gupta on 26-09-2016
    /// Description : Added MakeId, MakeMaskingName.
    /// </summary>
    [Serializable, DataContract]
    public class MostPopularBikesBase
    {
        [DataMember]
        public BikeMakeEntityBase objMake { get; set; }
        [DataMember]
        public BikeModelEntityBase objModel { get; set; }
        [DataMember]
        public BikeVersionsListEntity objVersion { get; set; }
        [DataMember]
        public string BikeName { get; set; }
        [DataMember]
        public string HostURL { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public int ReviewCount { get; set; }
        [DataMember]
        public double ModelRating { get; set; }
        [DataMember]
        public Int64 VersionPrice { get; set; }
        [DataMember]
        public MinSpecsEntity Specs { get; set; }
        [DataMember]
        public ushort BikePopularityIndex { get; set; }
        [DataMember]
        public int MakeId { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string MakeName { get; set; }
    }
}
