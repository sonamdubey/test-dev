using Carwale.Entity.UsedCarsDealer;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers.Used
{
    public interface IUsedDealerCitiesRepository
    {
        DealerCities GetCities(int dealerId);
        bool SaveCities(int dealerId, IEnumerable<int> cityIds);
    }
}
