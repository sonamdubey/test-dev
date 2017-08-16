using Bikewale.Entities.SEO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
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
        [DataMember]
        public bool IsScooterOnly { get; set; }
        [DataMember]
        public uint TotalCount { get; set; }
        [DataMember]
        public string Href { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public IEnumerable<CustomPageMetas> Metas { get; set; }
    }
}
