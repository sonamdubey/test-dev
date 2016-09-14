using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.UsedBikes
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 21st june 2016
    /// Desc : carrier of most recent bike details
    /// </summary>
    /// Modeified by:- Subodh jain 14 sep 2016
    [Serializable, DataContract]
    public class MostRecentBikes
    {
        [DataMember]
        public uint MakeYear { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string ModelMaskingName { get; set; }
        [DataMember]
        public string VersionName { get; set; }
        [DataMember]
        public uint BikePrice { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string ProfileId { get; set; }
        [DataMember]
        public uint AvailableBikes { get; set; }
        [DataMember]
        public string CityMaskingName { get; set; }
        [DataMember]
        public uint CityId { get; set; }

        [DataMember]
        public uint Kilometer { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}