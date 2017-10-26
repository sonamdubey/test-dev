using Newtonsoft.Json;

/// <summary>
/// Created by : Sangram Nandkhile on 09 oct
/// </summary>
namespace Bikewale.DTO.ManufacturerCampaign
{
    public class ManufacturerCampaignEMIConfigurationDTO
    {
        [JsonProperty("campaignId")]
        public uint CampaignId { get; set; }

        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

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

        [JsonProperty("emiButtonTextMobile")]
        public string EMIButtonTextMobile { get; set; }

        [JsonProperty("emiPropertyTextMobile")]
        public string EMIPropertyTextMobile { get; set; }

        [JsonProperty("emiButtonTextDesktop")]
        public string EMIButtonTextDesktop { get; set; }

        [JsonProperty("emiPropertyTextDesktop")]
        public string EMIPropertyTextDesktop { get; set; }

        [JsonProperty("pincodeRequired")]
        public bool PincodeRequired { get; set; }

        [JsonProperty("dealerRequired")]
        public bool DealerRequired { get; set; }

        [JsonProperty("emailRequired")]
        public bool EmailRequired { get; set; }

    }
}
