using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using Carwale.Interfaces.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.Geolocation
{
    public class LatLongCityRepository : ILatLongCityRepository
    {
        /// <summary>
        /// passing null value for city name and -1 for city id in  citybylatlong api
        /// written by Natesh kumar on 5/11/14
        /// </summary>
        public City GetCityDetailsByLatLong(LatLongURI querystring)
        {
            City result = new City();
            result.CityId = -1;
            result.CityName = null;

            return result;
        }
    }
}
