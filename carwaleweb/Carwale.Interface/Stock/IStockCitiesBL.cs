using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using System.Collections.Generic;

namespace Carwale.Interfaces.Stock
{
    public interface IStockCitiesBL
    {
        bool DeleteStockCities(int inquiryId, SellerType sellerType);
        bool AddStockCities(int inquiryId, SellerType sellerType, int mainCityId, IList<int> cityIds);
        IEnumerable<StockCity> GetStockCities(string profileId);
    }
}
