using Carwale.DTOs;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers.Used
{
    public interface IUsedDealerCitiesBL
    {
        List<City> GetCities(int dealerId);
        bool SaveCities(int dealerId, IEnumerable<int> cityIds);
    }
}
