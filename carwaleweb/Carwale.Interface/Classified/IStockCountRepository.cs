using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Carwale.Entity.Classified;

namespace Carwale.Interfaces.Classified
{
    public interface IStockCountRepository
    {
        UsedCarCount GetUsedCarsCount(int rootId, int cityId);
    }
}
