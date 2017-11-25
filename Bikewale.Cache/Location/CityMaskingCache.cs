using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections;

namespace Bikewale.Cache.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 7 June 2016
    /// Summary : Class has logic to get the new masking names from db and cache them.
    /// Logic used for 200 and 301 redirection of city masking names
    /// </summary>
    public class CityMaskingCache : ICityMaskingCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly ICity _cityRepository;

        public CityMaskingCache(ICacheManager cache, ICity cityRepository)
        {
            _cache = cache;
            _cityRepository = cityRepository;
        }

        public CityMaskingResponse GetCityMaskingResponse(string maskingName)
        {
            CityMaskingResponse response = new CityMaskingResponse();
            
            try
            {
                // Get MaskingNames from Memcache
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_NewCityMaskingNames", new TimeSpan(1, 0, 0), () => _cityRepository.GetMaskingNames());

                if (htNewMaskingNames.Contains(maskingName))
                {
                    response.CityId = Convert.ToUInt32(htNewMaskingNames[maskingName]);
                }

                // If modelId is not null 
                if (response.CityId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;

                    return response;
                }
                else
                {
                    // Get old MaskingNames from memcache
                    var htOldMaskingNames = _cache.GetFromCache<Hashtable>("BW_OldCityMaskingNames", new TimeSpan(1, 0, 0), () => _cityRepository.GetOldMaskingNames());

                    // new masking name found for given masking name. Its renamed so 301 permanant redirect.
                    if (htOldMaskingNames[maskingName] != null)
                    {
                        response.MaskingName = htOldMaskingNames[maskingName].ToString();
                        response.StatusCode = 301;
                    }
                    else
                        response.StatusCode = 404;                // Not found. The given masking name does not exist on bikewale.
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "CityMaskingCache.GetCityMaskingResponse");
                
            }

            return response;
        }
    }
}
