using BikewaleOpr.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
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

    public class DealerPriceQuoteList
    {
        public IEnumerable<DealerPriceQuoteDetailed> DealersDetails { get; set; }
    }
}
