using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Modified By : Sadhna Upadhayay on 06 Nov. 2015.
    /// Description : Introduce [Serializable].
    /// Modified By :   Sumit Kate on 29 Jan 2016
    /// Description :   Added HostUrl1, HostUrl2, VersionImgUrl1, VersionImgUrl2 as string to Bikewale.Entities.Compare.TopBikeCompareBase entity.
    /// Modified By : Sushil Kumar on 27th Oct 2016
    /// Description : Removed unused properties hosturl,reviews
    /// </summary>
    [Serializable, DataContract]
    public class TopBikeCompareBase
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public UInt16 VersionId1 { get; set; }
        [DataMember]
        public UInt16 VersionId2 { get; set; }
        [DataMember]
        public UInt16 ModelId1 { get; set; }
        [DataMember]
        public UInt16 ModelId2 { get; set; }
        [DataMember]
        public string Bike1 { get; set; }
        [DataMember]
        public string Bike2 { get; set; }
        [DataMember]
        public string MakeMaskingName1 { get; set; }
        [DataMember]
        public string MakeMaskingName2 { get; set; }
        [DataMember]
        public string ModelMaskingName1 { get; set; }
        [DataMember]
        public string ModelMaskingName2 { get; set; }
        [DataMember]
        public UInt32 Price1 { get; set; }
        [DataMember]
        public UInt32 Price2 { get; set; }
        [DataMember]
        public string HostUrl1 { get; set; }
        [DataMember]
        public string HostUrl2 { get; set; }
        [DataMember]
        public string VersionImgUrl1 { get; set; }
        [DataMember]
        public string VersionImgUrl2 { get; set; }
    }
}
