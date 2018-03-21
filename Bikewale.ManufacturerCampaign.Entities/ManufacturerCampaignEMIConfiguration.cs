using System;
namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacturer Campaign EMI Configuration
    /// Modified by : Pratibha Verma
    /// Description : Added property ShowOnExshowroom 
    /// </summary>
    [Serializable]
    public class ManufacturerCampaignEMIConfiguration
    {
        public uint CampaignId { get; set; }
        public uint DealerId { get; set; }
        public string Organization { get; set; }
        public string PopupHeading { get; set; }
        public string PopupDescription { get; set; }
        public string PopupSuccessMessage { get; set; }

        public string MaskingNumber { get; set; }
        public string EMIButtonTextMobile { get; set; }
        public string EMIPropertyTextMobile { get; set; }
        public string EMIButtonTextDesktop { get; set; }
        public string EMIPropertyTextDesktop { get; set; }

        public bool PincodeRequired { get; set; }
        public bool DealerRequired { get; set; }
        public bool EmailRequired { get; set; }
        public bool ShowOnExshowroom { get; set; }
    }
}
