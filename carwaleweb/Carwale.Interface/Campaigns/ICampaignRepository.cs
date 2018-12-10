using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Campaigns;
using Carwale.DTOs.Campaigns;
using Carwale.Entity.Dealers;
using Carwale.Entity.CarData;
using Carwale.Entity.Template;

namespace Carwale.Interfaces.Campaigns
{
    public interface ICampaignRepository
    {
        DealerInquiryDetails GetCampaignLeadInfo(int leadId);
        Dictionary<int, int> GetCampaignTemplateGroups(int campaignId, int platformId);
    }
}
