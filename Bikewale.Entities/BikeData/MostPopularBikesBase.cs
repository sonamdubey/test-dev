using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.GenericBikes;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By  : Sushil Kumar
    /// Description : Bike Entity to show most popular bikes by make
    /// Modified by : Sajal Gupta on 26-09-2016
    /// Description : Added MakeId, MakeMaskingName.
    /// Modified by : Ashutosh Sharma on 29 Sep 2017
    /// Description : Added AvgPrice and ExShowroomPrice.
    /// Modified by : Snehal Dange on 1st Feb 2018
    /// Description :  added EMIDetails
    /// Modified by : Snehal Dange on 6th Feb 2018
    /// Description : Added onroad price

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
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public EMI EMIDetails { get; set; }
        [DataMember]
        public Int64 OnRoadPrice { get; set; }
        [DataMember]
        public Int64 OnRoadPriceMumbai { get; set; }
        [IgnoreDataMember]
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
