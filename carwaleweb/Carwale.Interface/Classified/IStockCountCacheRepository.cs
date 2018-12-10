using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified
{
    public interface IStockCountCacheRepository
    {
        UsedCarCount GetUsedCarsCount(int rootId, int cityId);
    }
}
