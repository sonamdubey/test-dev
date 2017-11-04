using Bikewale.DTO.AutoBiz;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created by  :   Sumit Kate on 14 Mar 2016
    /// Description :   Dealer Quotation Entity
    /// </summary>
    public class DealerQuotationEntityDTO
    {
        [JsonProperty("isBookingAvailable")]
        public bool IsBookingAvailable { get { return (BookingAmount > 0); } }

        [JsonProperty("bookingAmount")]
        public uint BookingAmount { get; set; }

        [JsonProperty("priceList")]
        public IEnumerable<PQ_PriceDTO> PriceList { get; set; }

        [JsonProperty("offerList")]
        public IEnumerable<OfferEntityBaseDTO> OfferList { get; set; }

        [JsonProperty("dealerDetails")]
        public NewBikeDealersDTO DealerDetailsDTO { get; set; }

        [JsonProperty("bikeAvailability")]
        public uint BikeAvailabilityDTO { get; set; }

        [JsonProperty("availabilityByColor")]
        public IEnumerable<BikeColorAvailabilityDTO> AvailabilityByColor { get; set; }

        [JsonProperty("benefits")]
        public IEnumerable<DealerBenefitEntityDTO> Benefits { get; set; }

        [JsonProperty("emiDetails")]
        public EMIDTO EMIDetails { get; set; }

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