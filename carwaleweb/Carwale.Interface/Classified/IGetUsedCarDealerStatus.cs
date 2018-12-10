using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified
{
    public interface IGetUsedCarDealerStatus
    {
        String GetDealerStatus(int? SellerId);
        
    }
}
