using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Created By : Lucky Rathore on 21 March 2016
    /// Description : For caching of dealers list w.r.t. city and make. 
    /// </summary>
    public interface IDealerCacheRepository
    {
        Dealers GetDealerByMakeCity(uint cityId, uint makeId);
        DealerBikesEntity GetDealerBikes(UInt16 dealerId);
    }
}
