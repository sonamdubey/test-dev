using System;
namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jun 2017
    /// Description :   Manufacturer Campaign Lead Configuration
    /// Modifier    : Kartik Rathod on 16 may 2018,added SendLeadSMSCustomer 
    /// </summary>
    [Serializable]
    public class ManufacturerCampaignLeadConfiguration
    {
        public uint DealerId { get; set; }
        public uint CampaignId { get; set; }
        public string Organization { get; set; }
        public string PopupHeading { get; set; }
        public string PopupDescription { get; set; }
        public string PopupSuccessMessage { get; set; }

        public string MaskingNumber { get; set; }
        public string LeadsButtonTextMobile { get; set; }
        public string LeadsPropertyTextMobile { get; set; }
        public string LeadsButtonTextDesktop { get; set; }
        public string LeadsPropertyTextDesktop { get; set; }
        public string LeadsHtmlMobile { get; set; }
        public string LeadsHtmlDesktop { get; set; }

        public string PriceBreakUpLinkTextMobile { get; set; }
        public string PriceBreakUpLinkMobile { get; set; }
        public string PriceBreakUpLinkTextDesktop { get; set; }
        public string PriceBreakUpLinkDesktop { get; set; }

        public bool ShowOnExshowroom { get; set; }
        public bool PincodeRequired { get; set; }
        public bool DealerRequired { get; set; }
        public EnumEmailOptions EmailRequired { get; set; }
        public bool SendLeadSMSCustomer { get; set; }
    }
}
