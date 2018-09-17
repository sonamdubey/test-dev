using Bikewale.ManufacturerCampaign.Entities;
using Newtonsoft.Json;

namespace Bikewale.DTO.Campaign
{
    /// <summary>
    /// Author  : Kartik Rathod on 12 sept 2018
    /// Desc    : get campaign details for  Es campaings related to lead popup
    /// </summary>
    public class ManufacturerLeadCampaignDto
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
        public EnumEmailOptions EmailRequired { get; set; }
        [JsonProperty("sendLeadSMSCustomer")]
        public bool SendLeadSMSCustomer { get; set; }
    }
}
