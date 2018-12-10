using Carwale.Entity.Stock.Certification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Stock
{
    public interface IStockCertificationCacheRepository
    {
        StockCertification GetCarCertification(int inquiryId, bool isDealer);
        void RefreshCarCertification(int inquiryId, bool isDealer);
    }
}
