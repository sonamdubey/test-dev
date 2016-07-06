using System;

namespace BikewaleOpr.Entity
{
    public class OfferEntityBase
    {
        public UInt32 OfferId { get; set; }
        public UInt32 OfferCategoryId { get; set; }
        public string OfferType { get; set; }
        public string OfferText { get; set; }
        public UInt32 OfferValue { get; set; }
        public DateTime OffervalidTill { get; set; }
        public uint DealerId { get; set; }
    }
}
