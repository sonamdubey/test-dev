using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces
{
    public interface IStockImageRepository
    {
        int Create(CarPhotos entity);
        int Update(CarPhotos entity);
        bool Delete(CarPhotos entity);
        List<CarPhotos> GetProcessedImage(string imageIdList);
        bool UpdateStatus(StockImageStatus statusDetail, int stockId, int? sellerType);
        bool setMainImage(int inquiryid, int sellerType);
    }
}
