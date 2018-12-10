using Carwale.DTOs.Geolocation;
using Carwale.DTOs.Template;
using Carwale.Entity.Common;
using System.Collections.Generic;

namespace Carwale.DTOs.Campaigns
{
    public class EmiCalculatorDealerAdDto
    {
        public int ModelId { get; set; }
        public string CampaignDealerId { get; set; }
        public bool AdAvailable { get; set; }
        public CityAreaDTO UserLocation { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }

    }
}
