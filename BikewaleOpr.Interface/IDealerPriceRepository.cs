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
        DealerPriceBaseEntity GetDealerPrices(uint cityId, uint makeId, uint dealerId);
        bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList);
        bool SaveDealerPrice(uint dealerId, uint cityId, string versionIdList, string itemIdList, string itemvValueList, uint enteredBy);
    }
}
