using System;

namespace Bikewale.Entities
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 march 2016
    /// Summary : To get offers provided by Dealers.
    /// Updated by:Sangram Nandkhile on 25 Aug 2016
    /// Summary: added IsOfferTerms
    /// </summary>
    public class OfferEntityBase
    {
        public UInt32 OfferId { get; set; }
        public UInt32 OfferCategoryId { get; set; }
        public string OfferType { get; set; }
        public string OfferText { get; set; }
        public UInt32 OfferValue { get; set; }
        public DateTime OffervalidTill { get; set; }
        public uint DealerId { get; set; }
        public bool IsOfferTerms { get; set; }
    }
}
