using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Location
{
    public interface ICity
    {
        List<CityEntityBase> GetPriceQuoteCities(uint modelId);
        List<CityEntityBase> GetAllCities(EnumBikeType requestType);
        List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType);
    }
}
