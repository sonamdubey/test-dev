using System;

namespace Bikewale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Oct 2015
    /// </summary>
    public class OfferEntityBaseDTO
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
