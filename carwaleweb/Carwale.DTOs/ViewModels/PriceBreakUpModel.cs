using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Common;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.DTOs.ViewModels
{
    public class PriceBreakUpModel
    {
        public List<PQItemList> PricesList { get; set; }
        public PriceOverviewDTOV2 PriceOverview { get; set; }
        public string ModelName { get; set; }
        public int ModelId { get; set; }
        public string MakeName { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public string VersionName { get; set; }
        public int VersionId { get; set; }
        public int OnRoadPrice { get; set; }
        public bool IsCampaignAvailable { get; set; }
        public string Emi { get; set; }
        public bool ShowCampaignLink { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
        public EmiCalculatorModelLink EmiCalculatorModelLink { get; set; }
        public bool IsPicSnippetExperiment { get; set; }
        public string CampaignDealerId { get; set; }
        public CarOverviewDTO CarOverviewDto { get; set; }
        public string CampaignLeadCTA { get; set; }
    }
}
