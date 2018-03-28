using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BikeWale.Entities.AutoBiz
{
    [Serializable, DataContract]
    public class BikeColorAvailability
    {
        [DataMember]
        public uint ColorId { get; set; }
        [DataMember]
        public string ColorName { get; set; }
        [DataMember]
        public uint DealerId { get; set; }
        [DataMember]
        public short NoOfDays { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string HexCode { get; set; }
    }

    /// <summary>
    /// Created By : Sadhana Upadhyay on 11 Jan 2016
    /// Summary : To return list of bike color
    /// </summary>
    public class BikeAvailabilityByColor
    {
        public uint ColorId { get; set; }
        public string ColorName { get; set; }
        public uint DealerId { get; set; }
        public short NoOfDays { get; set; }
        public bool isActive { get; set; }
        public uint VersionId { get; set; }
        public IEnumerable<string> HexCode { get; set; }
    }
}
