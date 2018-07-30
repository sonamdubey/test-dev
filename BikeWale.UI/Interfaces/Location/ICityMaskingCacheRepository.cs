using Bikewale.Entities.Location;

namespace Bikewale.Interfaces.Location
{
    public interface ICityMaskingCacheRepository
    {
        CityMaskingResponse GetCityMaskingResponse(string maskingName);
    }
}
