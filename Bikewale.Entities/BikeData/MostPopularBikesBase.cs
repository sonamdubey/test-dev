using Bikewale.Entities.GenericBikes;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By  : Sushil Kumar
    /// Description : Bike Entity to show most popular bikes by make
    /// Modified by : Sajal Gupta on 26-09-2016
    /// Description : Added MakeId, MakeMaskingName.
    /// Modified by : Ashutosh Sharma on 29 Sep 2017
    /// Description : Added AvgPrice and ExShowroomPrice.
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
        public Int64 ExShowroomPrice { get; set; }
        [DataMember]
        public Int64 AvgPrice { get; set; }
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
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string CityMaskingName { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public EnumBikeBodyStyles BodyStyle { get; set; }
        [DataMember]
        public bool IsAdPromoted { get; set; }

    }
}
