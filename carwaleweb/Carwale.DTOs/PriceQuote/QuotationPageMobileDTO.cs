using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;
using Carwale.Entity.Template;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.DTOs.OfferAndDealerAd;

namespace Carwale.DTOs.PriceQuote
{
    public class QuotationPageMobileDTO
    {
        public PQCarDetails CarDetails { get; set; }
        public List<CarVersionEntity> Versions { get; private set; }
        public Location Location { get; set; }
        public List<Entity.Price.PriceQuote> PriceQuote { get; private set; }
        public NearByCityDetailsDto NearByCities { get; set; }
        public bool ShowSellCarLink { get; set; }
        public DealerAd DealerAd { get; set; }
        public bool IsCrossSellPriceQuote { get; set; }
        public int CardNo { get; set; }
        public bool IsCrossSellPresent { get; set; }
        public int FeaturedVersionId { get; set; }
        public int PageId { get; set; }
        public int OptionalChargesDisplayCount { get; set; }
        public OfferAndDealerAdDTO OfferAndDealerAd { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
        public string OffersCtaText { get; set; }
        public List<EmiCalculatorModelData> EmiCalculatorModelData { get; private set; }
        public Platform Source { get; set; }
        public bool HideCrossSellEmiSlug { get; set; }

        public QuotationPageMobileDTO()
        {
            Versions = new List<CarVersionEntity>();
            PriceQuote = new List<Entity.Price.PriceQuote>();
            EmiCalculatorModelData = new List<EmiCalculatorModelData>();
        }
    }
}
