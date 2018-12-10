using Carwale.DTOs.Campaigns;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class SponsoredDealerDTO
    {
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        [JsonProperty("dealerName")]
        public string DealerName { get; set; }
        [JsonProperty("dealerMobile")]
        public string DealerMobile { get; set; }
        [JsonProperty("dealerEmail")]
        public string DealerEmail { get; set; }
        [JsonProperty("dealerActualMobile")]
        public string DealerActualMobile { get; set; }
        [JsonProperty("dealerLeadBusinessType")]
        public int DealerLeadBusinessType { get; set; }
        [JsonProperty("templateHtml")]
        public string TemplateHtml { get; set; }
        [JsonProperty("templateName")]
        public string TemplateName { get; set; }
        [JsonProperty("leadPanel")]
        public int LeadPanel { get; set; }
        [JsonProperty("targetDealerId")]
        public int TargetDealerId { get; set; }
        [JsonProperty("templateHeight")]
        public int TemplateHeight { get; set; }
        [JsonProperty("dealerAddress")]
        public string DealerAddress { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("actualDealerId")]
        public int ActualDealerId { get; set; }
        [JsonProperty("linkText")]
        public string LinkText { get; set; }
        [JsonProperty("userSmsEnabled")]
        public bool UserSMSEnabled { get; set; }
        [JsonProperty("userEmailEnabled")]
        public bool UserEmailEnabled { get; set; }
        [JsonProperty("dealerSmsEnabled")]
        public bool DealerSMSEnabled { get; set; }
        [JsonProperty("dealerEmailEnabled")]
        public bool DealerEmailEnabled { get; set; }
        [JsonProperty("targetDealerEmail")]
        public string TargetDealerEmail { get; set; }
        [JsonProperty("targetDealerMobile")]
        public string TargetDealerMobile { get; set; }
        [JsonProperty("showEmail")]
        public bool ShowEmail { get; set; }
        [JsonProperty("dispSnippetOnDesk")]
        public string DispSnippetOnDesk { get; set; }
        [JsonProperty("dispSnippetOnMob")]
        public string DispSnippetOnMob { get; set; }
        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }
        [JsonProperty("leadSource")]
        public List<LeadSourceDTO> LeadSource { get; set; }
        [JsonProperty("predictionData")]
        public PredictionData PredictionData { get; set; }
    }
}
