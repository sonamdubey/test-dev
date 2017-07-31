using BikewaleOpr.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface
{
    public interface IDealerPriceRepository
    {
        DealerPriceBaseEntity GetDealerPrices(uint cityId, uint makeId, uint dealerId);
        bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList);
        bool SaveDealerPrice(uint dealerId, uint cityId, string versionIdList, string itemIdList, string itemvValueList, uint enteredBy);
    }
}
