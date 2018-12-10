using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Template;

namespace Carwale.Interfaces.Campaigns
{
    public interface ICampaignCacheRepository
    {
        Dictionary<int, int> GetCampaignGroupTemplateIdCache(int campaignId, int platformId);
    }
}
