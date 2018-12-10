using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Stock
{
    public interface IStockConditionRepository
    {
        bool AddStockCondition(int inquiryId, StockCondition carCondition);
        List<StockConditionItems> GetCarConditionParts();
        StockCondition GetCarConditionResponses(int inquiryId);
    }
}
