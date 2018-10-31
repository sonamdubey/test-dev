using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.DTO.Campaign
{
    public class ManufacturerEmiCampaignDto
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("popupHeading")]
        public string PopupHeading { get; set; }
        [JsonProperty("popupDescription")]
        public string PopupDescription { get; set; }
        [JsonProperty("popupSuccessMessage")]
        public string PopupSuccessMessage { get; set; }
        [JsonProperty("pincodeRequired")]
        public bool PincodeRequired { get; set; }
        [JsonProperty("dealerRequired")]
        public bool DealerRequired { get; set; }
        [JsonProperty("emailRequired")]
        public string EmailRequired { get; set; }
        [JsonProperty("sendLeadSMSCustomer")]
        public bool SendLeadSMSCustomer { get; set; }
        [JsonProperty("leadsButtonTextMobile")]
        public string LeadsButtonTextMobile { get; set; }
    }
}