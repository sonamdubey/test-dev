using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Dealer
{
    public class DealerInquiryDetailsDTO : CustomerBaseDTO
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("applicationId")]
        public int ApplicationId { get; set; }

        [JsonProperty("campaignId")]
        public int DealerId { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("zoneId")]
        public int ZoneId { get; set; }

        [JsonProperty("inquirySourceId")]
        public int InquirySourceId { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("leadClickSource")]
        public int LeadClickSource { get; set; }

        [JsonProperty("platformId")]
        public int PlatformSourceId { get; set; }

        [JsonProperty("dealerId")]
        public int AssignedDealerId { get; set; }

        [JsonProperty("referrerUrl")]
        public string Ltsrc { get; set; }

        [JsonProperty("utmaCookie")]
        public string UtmaCookie { get; set; }

        [JsonProperty("utmzCookie")]
        public string UtmzCookie { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty("abTest")]
        public UInt16 ABTest { get; set; }

        public bool NewLead { get; set; }

        public string BuyTimeText { get; set; }

        public int BuyTimeValue { get; set; }

        public int RequestType { get; set; }

        public string CityName { get; set; }
        
        public string ModelName { get; set; }

        public string VersionName { get; set; }

        public ulong PQId { get; set; }

        public string MakeName { get; set; }

        public int LeadBussinessType { get; set; }

        public ulong PQDealerAdLeadId { get; set; }

        public string EncryptedPQDealerAdLeadId { get; set; }

        public bool IsAutoApproved { get; set; }

        public int ActualDealerId { get; set; }
 
        public string Comments { get; set; }

        public string Platform { get; set; }
        
        public string LeadSourceName { get; set; }

        public string LeadSourceCategoryId { get; set; }

        public string LeadSourceId { get; set; }

        public int DealsStockId { get; set; }

        public bool IsPaymentSuccess { get; set; }

        public string ModelsHistory { get; set; }

        public bool IsPushToThirdParty { get; set; }

        public string CwCookie { get; set; }

        public int LeadPushSource { get; set; }

        public string SponsoredBannerCookie { get; set; }

        public bool? LeadDuplication { get; set; }
    }
}
