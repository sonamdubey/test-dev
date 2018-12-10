using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using System.Collections.Generic;
namespace Carwale.Interfaces.Stock
{
    public interface IStockCitiesRepository
    {
        bool DeleteStockCities(int inquiryId, SellerType sellerType);
        bool AddStockCities(int inquiryId, SellerType sellerType, IEnumerable<int> cities);
        IEnumerable<StockCity> GetStockCities(int inquiryId, SellerType sellerType);
    }
}
