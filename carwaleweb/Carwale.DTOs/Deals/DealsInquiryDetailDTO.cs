using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsInquiryDetailDTO
    {
        [JsonProperty("recordId")]
        public int RecordId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }

        [JsonProperty("customerCity")]
        public int CustomerCity { get; set; }

        [JsonProperty("stockId")]
        public int StockId { get; set; }

        [JsonProperty("responseId")]
        public ulong ResponseId { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("masterCityId")]
        public int MasterCityId { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("isPaid")]
        public string IsPaid { get; set; }

        [JsonProperty("multipleStockId")]
        public string MultipleStockId { get; set; }

        [JsonProperty("platformId")]
        public int PlatformId { get; set; }

        [JsonProperty("eagerness")]
        public int Eagerness { get; set; }

        [JsonProperty("abTestValue")]
        public int ABTestValue { get; set; }

        [JsonProperty("campaignId")]
        public int CampaignId { get; set; }

        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
    }
}
