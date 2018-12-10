using Carwale.Entity.CentralizedCacheRefresh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CentralizedCacheRefresh
{
   public interface ICentralizedCacheRefreshBl
    {
       bool RefreshCentralizedCache(CacheRefreshWrapper refreshWrapper);
    }
}
