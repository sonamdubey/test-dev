using Bikewale.Entities.DealerLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 21 March 2016
    /// Description : Used for functionality for Dealer Bikes.
    /// </summary>
    public interface IDealer
    {
        DealerBikesEntity GetDealerBikes(UInt16 dealerId);
    }
}
