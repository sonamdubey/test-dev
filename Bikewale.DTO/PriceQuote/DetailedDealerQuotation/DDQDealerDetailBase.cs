using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
