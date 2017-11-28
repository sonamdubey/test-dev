using System;

namespace Bikewale.Entities.Dealer
{
    public class Offer
    {
        public UInt32 OfferId { get; set; }
        public string OfferText { get; set; }
        public UInt32 OfferValue { get; set; }
        //public UInt32 OfferCategoryId { get; set; }
        //public string OfferType { get; set; }
        //public bool IsOfferTerms { get; set; }
        //public bool IsPriceImpact { get; set; }
    }
}
