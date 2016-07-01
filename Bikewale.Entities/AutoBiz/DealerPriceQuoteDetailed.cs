using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using BikeWale.Entities.AutoBiz;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 23 Oct 2015
    /// </summary>
    public class DealerPriceQuoteDetailed
    {
        [JsonProperty("bookingAmount")]
        public uint BookingAmount { get; set; }

        [JsonProperty("priceList")]
        public IEnumerable<PQ_Price> PriceList { get; set; }

        [JsonProperty("offerList")]
        public IEnumerable<OfferEntityBase> OfferList { get; set; }

        [JsonProperty("dealerDetails")]
        public NewBikeDealers DealerDetails { get; set; }

        [JsonProperty("availability")]
        public uint Availability { get; set; }

        [JsonProperty("availabilityByColor")]
        public IEnumerable<BikeAvailabilityByColor> AvailabilityByColor { get; set; }
    }
}