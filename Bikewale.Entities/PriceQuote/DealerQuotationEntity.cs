﻿using Bikewale.Entities.BikeBooking;
using Newtonsoft.Json;
using System.Collections.Generic;

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
        public bool IsBookingAvailable { get; set; }

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

        [JsonProperty("hasBenefits")]
        public bool HasBenefits { get; set; }
    }
}