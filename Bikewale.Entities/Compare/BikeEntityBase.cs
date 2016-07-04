using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created by  :   Sumit Kate on 22 Jan 2016
    /// Description :   
    /// </summary>
    [Serializable,DataContract]
    public class BikeEntityBase
    {
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string ModelMaskingName { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public UInt16 VersionRating { get; set; }
        [DataMember]
        public UInt16 ModelRating { get; set; }
    }
}
