using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Bikewale.Entities;
using Bikewale.DTO.AutoBiz;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 23 Oct 2015
    /// </summary>
    public class DealerPriceQuoteDetailedDTO
    {
        [JsonProperty("bookingAmount")]
        public uint BookingAmount { get; set; }

        [JsonProperty("priceList")]
        public IEnumerable<PQ_PriceDTO> PriceList { get; set; }

        [JsonProperty("offerList")]
        public IEnumerable<OfferEntityBaseDTO> OfferList { get; set; }

        [JsonProperty("dealerDetails")]
        public NewBikeDealersDTO DealerDetails { get; set; }

        [JsonProperty("availability")]
        public uint Availability { get; set; }

        [JsonProperty("availabilityByColor")]
        public IEnumerable<BikeAvailabilityByColorDTO> AvailabilityByColor { get; set; }
    }
}