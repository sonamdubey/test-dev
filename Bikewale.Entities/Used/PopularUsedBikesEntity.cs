using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.UsedBikes
{
    [Serializable, DataContract]
    public class PopularUsedBikesEntity
    {
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public uint TotalBikes { get; set; }
        [DataMember]
        public double AvgPrice { get; set; }
        [DataMember]
        public string HostURL { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public string CityMaskingName { get; set; }
    }
}