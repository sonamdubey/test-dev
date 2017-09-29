using System;
using System.Collections.Generic;

namespace Bikewale.ManufacturerCampaign.Entities
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 26 Jun 2017
    /// Summary    : Entity for lead, emi property and campaign
    /// </summary>
    /// 
    public class ConfigureCampaignEntity
    {
        public ManufacturerCampaignDetails DealerDetails { get; set; }
        public IEnumerable<ManufacturerCampaignPages> CampaignPages { get; set; }
    }

    public class PriorityEntity
    {
        public uint Id { get; set; }
        public string Status { get; set; }
        public Boolean IsSelected { get; set; }
    }

    public class CampaignPropertyEntity
    {
        public uint CampaignId { get; set; }
        public uint DealerId { get; set; }
        public CampaignLeadPropertyEntity Lead { get; set; }
        public CampaignEMIPropertyEntity EMI { get; set; }
        public IEnumerable<PriorityEntity> EMIPriority { get; set; }
        public IEnumerable<PriorityEntity> LeadPriority { get; set; }
        public NavigationWidgetEntity NavigationWidget { get; set; }
    }
    public class CampaignEMIPropertyEntity
    {
        public bool HasEmiProperties { get; set; }
        public string ButtonTextMobile { get; set; }
        public string PropertyTextMobile { get; set; }
        public string ButtonTextDesktop { get; set; }
        public string PropertyTextDesktop { get; set; }
        public string Priority { get; set; }
        public bool EnableProperty { get; set; }
    }

    public class CampaignLeadPropertyEntity
    {
        public bool HasLeadProperties { get; set; }
        public string ButtonTextMobile { get; set; }
        public string PropertyTextMobile { get; set; }
        public string ButtonTextDesktop { get; set; }
        public string PropertyTextDesktop { get; set; }
        public string HtmlMobile { get; set; }
        public string HtmlDesktop { get; set; }
        public string PriceBreakUpLinkHtmlMobile { get; set; }
        public string PriceBreakUpLinkHtmlDesktop { get; set; }
        public string Priority { get; set; }
        public bool EnableProperty { get; set; }
    }

}
