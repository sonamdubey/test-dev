using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Cache.Campaigns
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 13 Feb 2018
    /// Description : Class which includes all the cache functionality related to Dealer Campaigns
    /// </summary>
    public class Dealers
    {
        public static void ClearDealerBikes(int dealerId)
        {
            BwMemCache.ClearDealerBikes(Convert.ToUInt32(dealerId));
        }
    }
}
