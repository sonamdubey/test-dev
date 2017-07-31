using BikewaleOpr.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Performs all DAL operations for Manage Dealer Pricing page.
    /// </summary>
    public interface IDealerPriceRepository
    {
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
        /// Description :   Fetches bike price quotes for given parameters.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        DealerPriceBaseEntity GetDealerPrices(uint cityId, uint makeId, uint dealerId);
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
        /// Description :   Deletes bike price quotes for given parameters. Accepts a list of versionsIds.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIdList"></param>
        /// <returns></returns>
        bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList);
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Updates or inserts dealer pricing.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIdList"></param>
        /// <param name="itemIdList"></param>
        /// <param name="itemValueList"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        bool SaveDealerPrice(uint dealerId, uint cityId, string versionIdList, string itemIdList, string itemvValueList, uint enteredBy);
    }
}
