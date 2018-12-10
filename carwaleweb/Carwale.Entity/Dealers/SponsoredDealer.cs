using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Campaigns;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class SponsoredDealer
    {
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerMobile { get; set; }
        public string DealerEmail { get; set; }
        public string DealerActualMobile { get; set; }
        public int DealerLeadBusinessType { get; set; }
        public string TemplateHtml { get; set; }
        public string TemplateName { get; set; }
        public int LeadPanel { get; set; }
        public int TargetDealerId { get; set; }
        public int TemplateHeight { get; set; }
        public string DealerAddress { get; set; }
        public string MakeName { get; set; }
        public int ActualDealerId { get; set; }
        public string LinkText { get; set; }
        public bool UserSMSEnabled { get; set; }
        public bool UserEmailEnabled { get; set; }
        public bool DealerSMSEnabled { get; set; }
        public bool DealerEmailEnabled { get; set; }
        public string TargetDealerEmail { get; set; }
        public string TargetDealerMobile { get; set; }
        public bool ShowEmail { get; set; }
        public string DispSnippetOnDesk { get; set; }
        public string DispSnippetOnMob { get; set; }
        public string ShortDescription { get; set; }
        public PredictionModelResponse PredictionData { get; set; }
        public List<LeadSource> LeadSource { get; set; }
        public bool? LeadDuplication { get; set; }
        public int AssignedTemplateId { get; set; }
        public int AssignedGroupId { get; set; }
        public string CTALinkText { get; set; }
        public bool MaskingNumberEnabled { get; set; }
        public bool MutualLeads { get; set; }
        public int DealerAdminId { get; set; }
    }
}
