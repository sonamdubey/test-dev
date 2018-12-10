using Carwale.BL.Interface.Stock.Search;
using Carwale.DAL.Interface.Classified.ElasticSearch;
using Carwale.Entity.Elastic;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Geolocation;
using System;
using System.Collections.Generic;

namespace Carwale.BL.Stock.Search
{
    public class NearbyCityLogic : INearbyCityLogic
    {
        private const double _latitudeSecondPerKilometer = 32.57940665;
        private const double _longitudeSecondPerKilometer = 34.63696611;
        
        private const int _distance = 50;

        private readonly INearbyCityRepository _nearbyCityRepository;
        private readonly IGeoCitiesCacheRepository _geoCacheRepo;
        public NearbyCityLogic(INearbyCityRepository nearbyCityRepository, IGeoCitiesCacheRepository geoCacheRepo)
        {
            _nearbyCityRepository = nearbyCityRepository;
            _geoCacheRepo = geoCacheRepo;
        }
        public List<Carwale.Entity.Classified.City> GetCities(ElasticOuptputs filterInputs)
        {
            double maxLat = 0.0;
            double minLat = 0.0;
            double maxLong = 0.0;
            double minLong = 0.0;
            if (filterInputs == null)
            {
                return null;
            }
            string city = filterInputs.cities != null ? filterInputs.cities[0] : string.Empty;
            if (string.IsNullOrWhiteSpace(city))
            {
                return null;
            }
            int stockCity = city == "3000" ? 1 : (city == "3001" ? 10 : Convert.ToInt32(city));

            Cities cityDetails = _geoCacheRepo.GetCityDetailsById(stockCity);

            if (cityDetails == null)
            {
                return null;
            }

            maxLat = Convert.ToDouble(cityDetails.Lattitude) + _distance * _latitudeSecondPerKilometer;
            minLat = Convert.ToDouble(cityDetails.Lattitude) - _distance * _latitudeSecondPerKilometer;
            maxLong = Convert.ToDouble(cityDetails.Longitude) + _distance * _longitudeSecondPerKilometer;
            minLong = Convert.ToDouble(cityDetails.Longitude) - _distance * _longitudeSecondPerKilometer;

            return _nearbyCityRepository.GetFromLatLong(minLat, maxLat, minLong, maxLong, stockCity, filterInputs);

        }

        public string GetNearbyCityText(int cityId, int cityCount)
        {
            string cityName = _geoCacheRepo.GetCityNameById(cityId.ToString());
            string carCountText = cityCount == 1 ? $"{ cityCount} Car" : $"{ cityCount} Cars";
            return $"More Cars from { cityName } ({ carCountText })";
        }
    }
}
