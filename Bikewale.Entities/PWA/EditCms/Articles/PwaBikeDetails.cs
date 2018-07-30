using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    ///  Modified by: Dhruv Joshi  
    ///  Dated: 16th May 2018
    ///  Description: Added ModelName, ModelId, MakeName and IsNew properties
    /// </summary>
    [Serializable, DataContract]
    public class PwaBikeDetails
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DetailPageUrl { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string PriceSuffix { get; set; }
        [DataMember]
        public string PriceDescription { get; set; }
        [DataMember]
        public string ImgUrl { get; set; }
        [DataMember]
        public int ModelId{ get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public bool IsNew { get; set; }
    }
}
