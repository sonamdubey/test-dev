using Carwale.Entity.Stock.Certification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Stock
{
    public interface IStockCertificationRepository
    {
        StockCertification GetStockCertification(int inquiryId, bool isDealer);
        int AddStockCertification(StockCertification stockCertification);
        int UpdateStockCertification(StockCertification stockCertification);
        int UpdateStockCertificationStatus(int inquiryId, bool isDealer, bool isActive);
    }
}
