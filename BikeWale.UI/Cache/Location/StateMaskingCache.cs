using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;

namespace Bikewale.Cache.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : 
    /// </summary>
    public class tateMaskingCache : IStateMaskingCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IState _stateRepository;

        public StateMaskingCache(ICacheManager cache, IState stateRepository)
        {
            _cache = cache;
            _stateRepository = stateRepository;
        }

        public StateMaskingResponse GetStateMaskingResponse(string maskingName)
        {
            StateMaskingResponse response = new StateMaskingResponse();

            try
            {
                // Get MaskingNames from Memcache
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_NewStateMaskingNames", new TimeSpan(1, 0, 0), () => _stateRepository.GetMaskingNames());

                if (htNewMaskingNames.Contains(maskingName))
                {
                    response.StateId = Convert.ToUInt32(htNewMaskingNames[maskingName]);
                }

                // If modelId is not null 
                if (response.StateId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;

                    return response;
                }
                else
                    response.StatusCode = 404;                // Not found. The given masking name does not exist on bikewale.
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "StateMaskingCache.GetStateMaskingResponse");
                objErr.SendMail();
            }

            return response;
        }
    }
}
