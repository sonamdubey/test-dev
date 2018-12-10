using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class Campaign
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public short Type { get; set; }
        public bool ShowOnDesktop { get; set; }
        public bool ShowOnMobile { get; set; }
        public bool ShowOnAndroid { get; set; }
        public bool ShowOniOS { get; set; }
        public int LeadTarget { get; set; }
        public int LeadTargetAchieved { get; set; }
        public int DailyLeadTarget { get; set; }
        public int DailyLeadTargetAchieved { get; set; }
        public string ActionText { get; set; }
        public bool NotifyUserByEmail { get; set; }
        public bool NotifyUserBySMS { get; set; }
        public bool NotifyDealerByEmail { get; set; }
        public bool NotifyDealerBySMS { get; set; }
        public bool IsEmailRequired { get; set; }
        public short Priority { get; set; }
        public short LeadPanel { get; set; }
        public bool IsThirdPartyCampaign { get; set; }
        public bool IsFeaturedEnabled { get; set; }
        public bool ShowInRecommendation { get; set; }
        public PredictionModelResponse PredictionData { get; set; }
        public CvlDetails CvlDetails { get; set; }
        public bool LeadDuplication { get; set; }
        public int AssignedTemplateId { get; set; }
        public int AssignedGroupId { get; set; }
        public string CTALinkText { get; set; }
        public bool MaskingNumberEnabled { get; set; }
        public bool MutualLeads { get; set; }
        public int DealerAdminId { get; set; }
        public bool IsTestDriveCampaign { get; set; }
        public bool IsTurboMla { get; set; }

        public Campaign()
        {
            this.CvlDetails = new CvlDetails();
        }
    }

    [Serializable]
    public class CvlDetails
    {
        public bool IsCvl { get; set; }
    }
}
