using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity;

namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
    /// Description :   Performs all BAL operations for Manage Dealer Pricing page.
    /// </summary>
    public interface IDealerPrice
    {
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Fetches dealer pricings and performs grouping between version and category lists.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        IEnumerable<DealerVersionPriceEntity> GetDealerPriceQuotes(uint cityId, uint makeId, uint dealerId);
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Constructs comma seperated delimiter array and calls DealerPriceRepository for price deletion.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIds"></param>
        /// <returns></returns>
        bool DeleteVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds);
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Constructs comma seperated delimiter arrays and calls SaveDealerPrice for price updation or insertion.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIds"></param>
        /// <param name="itemIds"></param>
        /// <param name="itemValues"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        bool SaveVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy);
    }
}
