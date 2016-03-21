using Bikewale.Entities.DealerLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.DealerLocator
{
    public interface IDealer
    {
        DealerBikesEntity GetDealerBikes(UInt16 dealerId);
    }
}
