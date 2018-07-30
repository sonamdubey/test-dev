using Newtonsoft.Json;

namespace Bikewale.DTO.ManufacturerCampaign
{
    public class ManufacturerCampaignLeadConfigurationDTO
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

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("leadsButtonTextMobile")]
        public string LeadsButtonTextMobile { get; set; }

        [JsonProperty("leadsPropertyTextMobile")]
        public string LeadsPropertyTextMobile { get; set; }

        [JsonProperty("leadsButtonTextDesktop")]
        public string LeadsButtonTextDesktop { get; set; }

        [JsonProperty("leadsPropertyTextDesktop")]
        public string LeadsPropertyTextDesktop { get; set; }

        [JsonProperty("leadsHtmlMobile")]
        public string LeadsHtmlMobile { get; set; }

        [JsonProperty("leadsHtmlDesktop")]
        public string LeadsHtmlDesktop { get; set; }

        [JsonProperty("showOnExshowroom")]
        public bool ShowOnExshowroom { get; set; }

        [JsonProperty("pincodeRequired")]
        public bool PincodeRequired { get; set; }

        [JsonProperty("dealerRequired")]
        public bool DealerRequired { get; set; }

        [JsonProperty("emailRequired")]
        public bool EmailRequired { get; set; }

    }
}
