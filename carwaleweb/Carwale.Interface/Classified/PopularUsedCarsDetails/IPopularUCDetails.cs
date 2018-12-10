using Carwale.Entity.Classified.PopularUsedCars;
using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Classified.PopularUsedCarsDetails
{
    public interface IPopularUCDetails
    {
        List<T> GetPopularUsedCarDetails<T>(string cityId);
        List<PopularUsedCarModel> FillPopularUsedCarDetails(City city);
        City FetchCityById(string cityId);
    }
}
