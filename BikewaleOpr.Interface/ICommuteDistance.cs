using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity.DealerCampaign;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 22 May 2017
    /// Summmary : Interface have methods related tyo 
    /// </summary>
    public interface ICommuteDistance
    {
        bool SaveCampaignAreas(uint dealerId, uint campaignid, ushort campaignServingStatus, ushort servingRadius, string cityIdList, string stateIdList);
        bool SaveAdditionalCampaignAreas(uint dealerId, string areaIdList);
        bool UpdateCommuteDistance(uint dealerId, DealerAreaDistance objDealerAreaDist);
    }
}
