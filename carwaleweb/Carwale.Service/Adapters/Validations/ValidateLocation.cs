using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Validations;

namespace Carwale.Service.Adapters.Validations
{
    public class ValidateLocation : IValidateLocation
    {
        private readonly IGeoCitiesCacheRepository _geoCitiesCache;

        public ValidateLocation(IGeoCitiesCacheRepository geoCitiesCache)
        {
            _geoCitiesCache = geoCitiesCache;
        }

        public bool IsCityValid(int cityId)
        {
            if (cityId > 0)
            {
                var cityDetails = _geoCitiesCache.GetCityDetailsById(cityId);
                if (cityDetails != null && !cityDetails.IsDeleted)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
