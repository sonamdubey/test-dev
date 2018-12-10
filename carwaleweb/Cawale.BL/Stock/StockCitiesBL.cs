using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Stock
{
    public class StockCitiesBL: IStockCitiesBL 
    {
        private readonly IStockCitiesRepository _stockCitiesRepository;
        private readonly IStockBL _stockBL;

        public StockCitiesBL(IStockCitiesRepository stockCitiesRepository, IStockBL stockBL)
        {
            _stockCitiesRepository = stockCitiesRepository;
            _stockBL = stockBL;
        }

        public bool DeleteStockCities(int inquiryId, SellerType sellerType)
        {
            return _stockCitiesRepository.DeleteStockCities(inquiryId, sellerType);
        }

        public bool AddStockCities(int inquiryId, SellerType sellerType, int mainCityId, IList<int> cityIds)
        {
            if (!cityIds.Any(cityId => cityId == mainCityId))
                cityIds.Add(mainCityId);

            return _stockCitiesRepository.AddStockCities(inquiryId, sellerType, cityIds);
        }

        public IEnumerable<StockCity> GetStockCities(string profileId)
        {
            IEnumerable<StockCity> cities = null;
            CarDetailsEntity stock = _stockBL.GetStock(profileId);
            if (stock == null || stock.BasicCarInfo == null || stock.IsSold)
            {
                return cities;
            }
            cities = _stockCitiesRepository.GetStockCities(StockBL.GetInquiryId(profileId), StockBL.IsDealerStock(profileId) ? SellerType.Dealer : SellerType.Individual);
            if (cities == null || !cities.Any())
            {
                cities = new List<StockCity>
                            {
                                new StockCity
                                {
                                    CityId = (int)stock.BasicCarInfo.CityId,
                                    CityName = stock.BasicCarInfo.CityName
                                }
                            };
            }
            return cities;
        }
    }
}
