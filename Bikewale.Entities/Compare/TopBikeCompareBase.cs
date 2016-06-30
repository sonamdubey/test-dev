using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Modified By : Sadhna Upadhayay on 06 Nov. 2015.
    /// Description : Introduce [Serializable].
    /// Modified By :   Sumit Kate on 29 Jan 2016
    /// Description :   Added HostUrl1, HostUrl2, VersionImgUrl1, VersionImgUrl2 as string to Bikewale.Entities.Compare.TopBikeCompareBase entity.
    /// </summary>
    [Serializable,DataContract]
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
        public UInt16 Review1 { get; set; }
        [DataMember]
        public UInt16 Review2 { get; set; }
        [DataMember]
        public UInt16 ReviewCount1 { get; set; }
        [DataMember]
        public UInt16 ReviewCount2 { get; set; }
        [DataMember]
        public string HostURL { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
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
