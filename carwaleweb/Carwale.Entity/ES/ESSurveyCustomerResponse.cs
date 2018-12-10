using Carwale.Entity.Customers;
using Carwale.Entity.Enum;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Carwale.Entity.ES
{
    [Serializable, JsonObject]
    public class ESSurveyCustomerResponse
    {
        [JsonProperty]
        public int CustomerId { get; set; }
        public CustomerMinimal BasicInfo { get; set; }

        [JsonProperty]
        public string SurveyResponse { get; set; }

        [JsonProperty]
        public Platform Platform { get; set; }

        [JsonProperty]
        public int CampaignId { get; set; }

        [JsonProperty]
        public string Comment { get; set; }

        [JsonProperty]
        public string Answer { get; set; }

        [JsonProperty]
        public bool IsFreeTextResponse { get; set; }

        [JsonProperty]
        public int CityId { get; set; }

        [JsonProperty]
        public string Address { get; set; }
    }
}
