using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity;

namespace BikewaleOpr.Interface
{
    public interface IDealerPrice
    {
        IEnumerable<DealerVersionPriceEntity> GetDealerPriceQuotes(uint cityId, uint makeId, uint dealerId);
        bool DeleteVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds);
        bool SaveVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy);
    }
}
