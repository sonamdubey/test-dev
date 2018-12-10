using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Campaigns
{
    /// <summary>
    /// For getting alternate Cars with active campaigns on submitting a lead
    /// Created: Vicky Lund, 01/12/2015
    /// </summary>
    /// <returns></returns>
    public class CampaignRecommendationEntity
    {
        [JsonProperty("carMake")]
        public CarMakesDTO CarMake;

        [JsonProperty("carModel")]
        public CarModelsDTO CarModel;

        [JsonProperty("carPrices")]
        public CarPricesDTO CarPrices;

        [JsonProperty("custLocation")]
        public CustLocationDTO CustLocation;

        [JsonProperty("carImageBase")]
        public CarImageBaseDTO CarImageBase;

        [JsonProperty("dealerSummary")]
        public DealerSummaryDTO DealerSummary;

        [JsonProperty("campaignId")]
        public int CampaignId { get; set; }

        [JsonProperty("source")]
        public int Source { get; set; }

        [JsonProperty("carPricesOverview")]
        public PriceOverviewDTO PricesOverview { get; set; }
    }
}
