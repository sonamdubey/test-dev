using Bikewale.Entities.BikeBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 march 2016
    /// Summary : To wrap response for Dealer Quotation page.
    /// </summary>
    public class DealerQuotationEntity
    {
        [JsonProperty("isBookingAvailable")]
        public bool IsBookingAvailable { get { return (BookingAmount > 0); } }

        [JsonProperty("bookingAmount")]
        public uint BookingAmount { get; set; }

        [JsonProperty("priceList")]
        public IEnumerable<PQ_Price> PriceList { get; set; }

        [JsonProperty("offerList")]
        public IEnumerable<OfferEntityBase> OfferList { get; set; }

        [JsonProperty("dealerDetails")]
        public NewBikeDealers DealerDetails { get; set; }

        [JsonProperty("bikeAvailability")]
        public uint BikeAvailability { get; set; }

        [JsonProperty("benefits")]
        public IEnumerable<DealerBenefitEntity> Benefits { get; set; }

        [JsonProperty("emiDetails")]
        public EMI EMIDetails { get; set; }

        [JsonProperty("availabilityByColor")]
        public IEnumerable<BikeWale.Entities.AutoBiz.BikeColorAvailability> AvailabilityByColor { get; set; }

        [JsonProperty("disclaimer")]
        public IEnumerable<string> Disclaimer { get; set; }

        [JsonProperty("hasDisclaimer")]
        public bool HasDisclaimer { get { return (Disclaimer != null ? Disclaimer.Count() : 0) > 0; } }

        [JsonProperty("hasBenefits")]
        public bool HasBenefits { get { return (Benefits != null ? Benefits.Count() : 0) > 0; } }

        [JsonProperty("totalPrice")]
        public ulong TotalPrice { get { return (PriceList != null ? Convert.ToUInt64(PriceList.Sum(m => Convert.ToInt64(m.Price))) : 0UL); } }
    }
}