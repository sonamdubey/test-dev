using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Location
{
    public interface ICityCacheRepository
    {
        IEnumerable<Entities.Location.CityEntityBase> GetPriceQuoteCities(uint modelId);
    }
}
