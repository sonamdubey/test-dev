using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using System.Collections.Generic;

namespace Carwale.Interfaces.Classified
{
    public interface ICommonOperationsCacheRepository
    {
        IList<DealerCityEntity> GetLiveListingCities();
        IList<CarMakeEntityBase> GetLiveListingMakes();
        CarModelMaskingResponse GetMakeDetailsByRootName(string rootName);
    }
}
