using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 march 2016
    /// Summary : To get offers provided by Dealers.
    /// Updated by:Sangram Nandkhile on 25 Aug 2016
    /// Summary: added IsOfferTerms
    /// </summary>
    [Serializable, DataContract]
    public class OfferEntityBase
    {
        [DataMember]
        public UInt32 OfferId { get; set; }
        [DataMember]
        public UInt32 OfferCategoryId { get; set; }
        [DataMember]
        public string OfferType { get; set; }
        [DataMember]
        public string OfferText { get; set; }
        [DataMember]
        public UInt32 OfferValue { get; set; }
        [DataMember]
        public DateTime OffervalidTill { get; set; }
        [DataMember]
        public uint DealerId { get; set; }
        [DataMember]
        public bool IsOfferTerms { get; set; }
    }
}
