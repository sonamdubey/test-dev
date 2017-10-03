using System.Web.Mvc;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 22 Jun 2017
    /// Summary: View Model for Configure Campaign Properties
    /// </summary>
    public class CampaignPropertiesVM
    {
        public uint CampaignId { get; set; }
        public bool HasEmiProperties { get; set; }
        public string EmiButtonTextMobile { get; set; }
        public string EmiPropertyTextMobile { get; set; }
        public string EmiButtonTextDesktop { get; set; }
        public string EmiPropertyTextDesktop { get; set; }
        public string EmiPriority { get; set; }

        public bool HasLeadProperties { get; set; }
        public string LeadButtonTextMobile { get; set; }
        public string LeadPropertyTextMobile { get; set; }
        public string LeadButtonTextDesktop { get; set; }
        public string LeadPropertyTextDesktop { get; set; }
        [AllowHtml]
        public string LeadHtmlMobile { get; set; }
        [AllowHtml]
        public string LeadHtmlDesktop { get; set; }
        [AllowHtml]
        public string FormattedHtmlDesktop { get; set; }
        [AllowHtml]
        public string FormattedHtmlMobile { get; set; }

        public string PriceBreakUpLinkTextMobile { get; set; }
        public string PriceBreakUpLinkTextDesktop { get; set; }
        public string PriceBreakUpLinkMobile { get; set; }
        public string PriceBreakUpLinkDesktop { get; set; }

        public string LeadPriority { get; set; }
    }
}
