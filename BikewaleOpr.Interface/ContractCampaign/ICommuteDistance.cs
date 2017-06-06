using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity.DealerCampaign;

namespace BikewaleOpr.Interface.ContractCampaign
{
    public interface ICommuteDistance
    {
        bool SaveCampaignAreas(uint dealerId, ushort campaignServingStatus, ushort servingRadius, string cityIdList);
        bool SaveAdditionalCampaignAreas(uint dealerId, string areaIdList);
        bool UpdateCommuteDistance(uint dealerId, DealerAreaDistance objDealerAreaDist);
    }
}
