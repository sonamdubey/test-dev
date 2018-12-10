using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Insurance;
using Carwale.Entity.Offers;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class PQDesktop
    {
        public ulong PQId { get; set; }
        public string EnId { get; set; }
        public List<PQItem> PriceQuoteList { get; set; }
        public PQCarDetails carDetails { get; set; }
        public CustLocation cityDetail { get; set; }
        public List<CarVersionEntity> CarVersions { get; set; }
        public SponsoredDealer SponsoredDealer { get; set; }
        public List<SimilarCarModels> AlternateCars { get; set; }
        public List<PriceQuoteCityDTO> PQCities { get; set; }
        public List<CityZonesDTO> PQZones { get; set; }
        public InsuranceDiscount InsuranceDiscount { get; set; }
        public bool IsSponsoredCar { get; set; }
        public PQOfferEntity Offers { get; set; }
        public DealerShowroomDetails DealerShowroom { get; set; }
        public List<CrossSellCampaign> CrossSellCampaignList { get; set; }
        public DiscountSummary discountSummary { get; set; }
        public bool ShowSellCarLink { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
        public OfferAndDealerAdDTO OfferAndDealerAd { get; set; }
    }
}
