using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsSummaryDesktop_DTO
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

        [JsonProperty("stockCount")]
        public int StockCount { get; set; }

        [JsonProperty("carImageDetails")]
        public CarImageBaseDTO CarImageDetails { get; set; }

        [JsonProperty("isOfferAvailable")]
        public Boolean IsOfferAvailable { get; set; }

        [JsonProperty("offerValue")]
        public int OfferValue { get; set; }

        [JsonProperty("offers")]
        public string Offers { get; set; }

        [JsonProperty("extraSavings")]
        public int ExtraSavings { get; set; }

        [JsonProperty("offerText")]
        public string OfferText { get; set; }

        [JsonProperty("versionCount")]
        public string VersionCount { get; set; }

        [JsonProperty("specificationsOverview")]
        public string SpecificationsOverview { get; set; }
    }
}
