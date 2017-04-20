
using System;
using System.Runtime.Serialization;
namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Summary : HostUrl and LogoUrl Added
    /// Modified by :   Sumit Kate on 03 Mar 2016
    /// Description :   Added PopularityIndex
    /// </summary>
    [Serializable, DataContract]
    public class BikeMakeEntityBase
    {
        [DataMember]
        public int MakeId { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string LogoUrl { get; set; }
        [DataMember]
        public ushort PopularityIndex { get; set; }
    }
}
