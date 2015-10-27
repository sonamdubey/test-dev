using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;

namespace Bikewale.Cache.BikeData
{
    public class BikeModelMaskingCache<T, U> : IBikeMaskingCacheRepository<T, U> where T : BikeModelEntity, new()
    {
        private readonly ICacheManager _cache;
        private readonly IBikeModelsRepository<T,U> _modelsRepository;

        public BikeModelMaskingCache(ICacheManager cache, IBikeModelsRepository<T,U> modelsRepository)
        {
            _cache = cache;
            _modelsRepository = modelsRepository;
        }

        public ModelMaskingResponse GetModelMaskingResponse(string maskingName)
        {
            ModelMaskingResponse response = new ModelMaskingResponse();
            
            try
            {
                // Get MaskingNames from Memcache
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_NewModelMaskingNames", new TimeSpan(1, 0, 0), () => _modelsRepository.GetMaskingNames());

                if (htNewMaskingNames.Contains(maskingName))
                {
                    response.ModelId = Convert.ToUInt32(htNewMaskingNames[maskingName]);
                }

                // If modelId is not null 
                if (response.ModelId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;

                    return response;
                }
                else
                {
                    // Get old MaskingNames from memcache
                    var htOldMaskingNames = _cache.GetFromCache<Hashtable>("BW_OldModelMaskingNames", new TimeSpan(1, 0, 0), () => _modelsRepository.GetOldMaskingNames());

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
                ErrorClass objErr = new ErrorClass(ex, "GetModelMaskingResponse");
                objErr.SendMail();
            }

            return response;
        }
    }
}
