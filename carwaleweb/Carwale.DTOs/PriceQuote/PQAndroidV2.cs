using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.Offers;
using Carwale.DTOs.OffersV1;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Carwale.DTOs.PriceQuote
{
    public class PQAndroidV2
    {
        [JsonProperty("inquiryId")]
        public ulong InquiryId { get; set; }

        [JsonProperty("priceQuoteList")]
        public List<PQItemDTO> PriceQuoteList { get; set; }

        [JsonProperty("carDetails")]
        public CarDetailsDTO CarDetails;

        [JsonProperty("otherVersions")]
        public List<Versions> OtherVersions { get; set; }

        [JsonProperty("cityDetails")]
        public PQCustLocationDTO CityDetails;

        [JsonProperty("sponsoredDealer")]
        public SponsoredDealerDTO SponsoredDealer { get; set; }

        [JsonProperty("formatedonRoadPrice")]
        public string FormatedonRoadPrice { get; set; }

        [JsonProperty("onRoadPrice")]
        public long OnRoadPrice { get; set; }

        [JsonProperty("alternativeCars")]
        public List<SimilarCarModelsDTO> AlternativeCars { get; set; }

        [JsonProperty("offers")]
        public PQOffersDTO Offers { get; set; }

        [JsonProperty("dealerList")]
        public List<DealerDTO> DealerList { get; set; }

        [JsonProperty("specialZones")]
        public List<PuneThaneZonesDTO> SpecialZones { get; set; }

        [JsonProperty("isSponsoredCar")]
        public bool IsSponsoredCar { get; set; }

        [JsonProperty("linkedSponsoredCar")]
        public ulong LinkedSponsoredCar { get; set; }

        [JsonProperty("offerV1")]
        public OfferDto OfferV1 { get; set; }
    }
}
