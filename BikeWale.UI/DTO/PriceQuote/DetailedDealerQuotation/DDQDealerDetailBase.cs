using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Dealer detail quotation base
    /// Author  :   Sumit Kate
    /// Date    :   24 Aug 2015
    /// </summary>
    public class DDQDealerDetailBase
    {
        [JsonProperty("dealer")]
        public DDQNewBikeDealer objDealer { get; set; }
        [JsonProperty("quotation")]
        public DDQQuotationBase objQuotation { get; set; }
        [JsonProperty("offers")]
        public List<DDQOfferBase> objOffers { get; set; }
        [JsonProperty("facilities")]
        public List<DDQFacilityBase> objFacilities { get; set; }
        [JsonProperty("emi")]
        public DDQEMI objEmi { get; set; }
        [JsonProperty("bookingAmount")]
        public DDQBookingAmountBase objBookingAmt { get; set; }
        [JsonProperty("isInsuranceFree")]
        public bool IsInsuranceFree { get; set; }
        [JsonProperty("insuranceAmount")]
        public uint InsuranceAmount { get; set; }

    }
}
