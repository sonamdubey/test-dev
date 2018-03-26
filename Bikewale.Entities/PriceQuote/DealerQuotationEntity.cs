using Bikewale.Entities.BikeBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 march 2016
    /// Summary : To wrap response for Dealer Quotation page.
    /// </summary>
    [Serializable, DataContract]
    public class DealerQuotationEntity
    {

        [JsonProperty("bookingAmount"), DataMember]
        public uint BookingAmount { get; set; }

        [JsonProperty("isBookingAvailable"), DataMember]
        public bool IsBookingAvailable { get { return (BookingAmount > 0); } }

        [JsonProperty("priceList"), DataMember]
        public IEnumerable<PQ_Price> PriceList { get; set; }

        [JsonProperty("offerList"), DataMember]
        public IEnumerable<OfferEntityBase> OfferList { get; set; }

        [JsonProperty("dealerDetails"), DataMember]
        public NewBikeDealers DealerDetails { get; set; }

        [JsonProperty("bikeAvailability"), DataMember]
        public uint BikeAvailability { get; set; }

        [JsonProperty("benefits"), DataMember]
        public IEnumerable<DealerBenefitEntity> Benefits { get; set; }

        [JsonProperty("emiDetails"), DataMember]
        public EMI EMIDetails { get; set; }

        [JsonProperty("availabilityByColor"), DataMember]
        public IEnumerable<BikeWale.Entities.AutoBiz.BikeColorAvailability> AvailabilityByColor { get; set; }

        [JsonProperty("disclaimer"), DataMember]
        public IEnumerable<string> Disclaimer { get; set; }

        [JsonProperty("hasDisclaimer"), DataMember]
        public bool HasDisclaimer { get { return (Disclaimer != null && Disclaimer.Any()); } }

        [JsonProperty("hasBenefits"), DataMember]
        public bool HasBenefits { get { return (Benefits != null && Benefits.Any()); } }

        [DataMember]
        public bool HasOffers { get { return (OfferList != null && OfferList.Any()); } }

        [JsonProperty("totalPrice"), DataMember]
        public ulong TotalPrice { get { return (PriceList != null ? Convert.ToUInt64(PriceList.Sum(m => Convert.ToInt64(m.Price))) : 0UL); } }

        [DataMember]
        public bool IsPremiumDealer
        {
            get
            {
                return (this.DealerDetails != null
                    && (DealerPackageTypes.Premium == this.DealerDetails.DealerPackageType
                    || DealerPackageTypes.CPS == this.DealerDetails.DealerPackageType
                    )
                    );
            }
        }
        [DataMember]
        public bool IsStandardDealer { get { return (this.DealerDetails != null && DealerPackageTypes.Standard == this.DealerDetails.DealerPackageType); } }
        [DataMember]
        public bool IsDeluxDealer { get { return (this.DealerDetails != null && DealerPackageTypes.Deluxe == this.DealerDetails.DealerPackageType); } }

    }
}