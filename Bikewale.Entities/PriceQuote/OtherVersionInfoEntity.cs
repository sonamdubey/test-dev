using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By     :   Sumit Kate
    /// Modified Date   :   08 Oct 2015
    /// Desciption      :   Added the following properties
    ///                     Price,RTO,Insurance
    /// </summary>
    [Serializable, DataContract]
    public class OtherVersionInfoEntity
    {
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
        [DataMember]
        public ulong OnRoadPrice { get; set; }
        [DataMember]
        public UInt32 Price { get; set; }
        [DataMember]
        public UInt32 RTO { get; set; }
        [DataMember]
        public UInt32 Insurance { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
    }
}
