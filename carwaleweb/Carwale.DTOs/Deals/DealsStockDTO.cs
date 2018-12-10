using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsStockDTO
    {
        [JsonProperty("make")]
        public CarMakesDTO Make { get; set; }

        [JsonProperty("model")]
        public CarModelsDTO Model { get; set; }

        [JsonProperty("version")]
        public CarVersionsDTO Version { get; set; }

        [JsonProperty("savings")]
        public int Savings { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("OnRoadPrice")]
        public int OnRoadPrice { get; set; }

        [JsonProperty("OfferPrice")]
        public int OfferPrice { get; set; }

        [JsonProperty("offers")]
        public string Offers { get; set; }

        [JsonProperty("stockCount")]
        public int StockCount { get; set; }

        [JsonProperty("carImageDetails")]
        public CarImageBaseDTO CarImageDetails { get; set; }

        [JsonProperty("specificationsOverview")]
        public string SpecificationsOverview { get; set; }

        [JsonProperty("fuelType")]
        public int FuelType { get; set; }

        [JsonProperty("transmissionType")]
        public int TransmissionType { get; set; }

        [JsonProperty("stockId")]
        public int StockId { get; set; }

        [JsonProperty("percentSaving")]
        public int PercentSaving { get; set; }
        
        [JsonProperty("isSimilarCar")]
        public bool IsSimilarCar { get; set; }

        [JsonProperty("offerValue")]
        public int OfferValue { get; set; }

        [JsonProperty("extraSavings")]
        public int ExtraSavings { get; set; }

        [JsonProperty("versionSlugText")]
        public string VersionSlugText { get; set; }

        [JsonProperty("isAdvantageVersion")]
        public bool IsAdvantageVersion { get; set; }

        [JsonProperty("color")]
        public CarColorDTO Color { get; set; }

        [JsonProperty("manufacturingYear")]
        public int ManufacturingYear{get;set;}

        [JsonProperty("dealerId")]
        public int DealerId { get; set; }

        [JsonProperty("campaignId")]
        public int CampaignId { get; set; }
    }
}
