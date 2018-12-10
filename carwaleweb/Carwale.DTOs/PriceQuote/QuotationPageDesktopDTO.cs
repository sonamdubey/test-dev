using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.LeadForm;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.Entity;
using Carwale.Entity.Common;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class QuotationPageDesktopDTO
    {
        public CarDetailsDTO CarDetails { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public List<VersionSpecPriceDto> VersionList { get; set; }
        public OfferAndDealerAdDTO OfferAndDealerAd { get; set; }
        public PQCustLocationDTO Location { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public NearByCityDetailsDto NearByCities { get; set; }
        public List<SimilarCarModelsDtoV3> SimilarModelList { get; set; }
        public PriceDetailsDto SolidCompulsoryList { get; set; }
        public PriceDetailsDto MetallicCompulsoryList { get; set; }
        public OptionalPricesDto SolidOptionalList { get; set; }
        public OptionalPricesDto MetallicOptionalList { get; set; }
        public List<CrossSellDto> CrossSellList { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
        public bool IsCampaignPresent { get; set; }
        public bool IsOfferPresent { get; set; }
        public bool ShowSellCarOneThirdSlug { get; set; }
        public bool ShowSellCarTwoThirdSlug { get; set; }
        public bool IsSolidCompulsoryPricePresent { get; set; }
        public bool IsMetallicCompulsoryPricePresent { get; set; }
        public bool IsSolidOptionalPricePresent { get; set; }
        public bool IsMetallicOptionalPricePresent { get; set; }
        public AlternateCarsDTO AlternateCars { get; set; }
        public List<EmiCalculatorModelLink> EmiCalculatorModelLink { get; set; }
        public LeadFormModelData LeadFormModelData { get; set; }

        public QuotationPageDesktopDTO()
        {
            this.SolidCompulsoryList = new PriceDetailsDto();
            this.MetallicCompulsoryList = new PriceDetailsDto();
            this.SolidOptionalList = new OptionalPricesDto();
            this.MetallicOptionalList = new OptionalPricesDto();
            this.VersionList = new List<VersionSpecPriceDto>();
            this.EmiCalculatorModelLink = new List<EmiCalculatorModelLink>();
            this.CrossSellList = new List<CrossSellDto>();
        }
    }
}
