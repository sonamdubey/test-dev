using Carwale.DTOs;
using Carwale.Entity.UsedCarsDealer;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Interfaces.Stock;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Dealers.Used
{
    public class UsedDealerCitiesBL : IUsedDealerCitiesBL
    {
        private readonly IUsedDealerCitiesRepository _usedDealerCitiesRepository;
        private readonly IStockBL _stockBL;

        public UsedDealerCitiesBL(IUsedDealerCitiesRepository usedDealerCitiesRepository, IStockBL stockBL)
        {
            _usedDealerCitiesRepository = usedDealerCitiesRepository;
            _stockBL = stockBL;
        }
        public List<City> GetCities(int dealerId)
        {
            DealerCities dealerCities = _usedDealerCitiesRepository.GetCities(dealerId);
            List<City> listOfDealerCities = new List<City>();
            if (dealerCities == null || dealerCities.CityIds == null || dealerCities.CityNames == null)
            {
                return listOfDealerCities;
            }

            List<string> listOfIds = dealerCities.CityIds.Split(',').ToList();
            List<string> listOfNames = dealerCities.CityNames.Split(',').ToList();
            for (int i = 0; i < listOfIds.Count; i++)
            {
                listOfDealerCities.Add(new City()
                {
                    CityId = Int32.Parse(listOfIds[i]),
                    CityName = listOfNames[i]
                });
            }
            return listOfDealerCities;
        }
        public bool SaveCities(int dealerId, IEnumerable<int> cityIds)
        {
            cityIds = cityIds?.Distinct().OrderBy(x => x);
            bool isEveryCityValid = _usedDealerCitiesRepository.SaveCities(dealerId, cityIds);
            if (isEveryCityValid)//true if cityIds is null
            {
                _stockBL.RefreshESStockOfDealer(dealerId);
            }
            return isEveryCityValid;
        }
    }
}
