using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers
{
    public interface IDealerSponsoredAdCache
    {
        List<NewCarDealersList> GetNewCarDealersByMakeAndCityId(int makeId, int cityId);
    }
}
