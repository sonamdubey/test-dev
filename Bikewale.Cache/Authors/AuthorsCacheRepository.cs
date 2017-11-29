using Bikewale.Entities.Authors;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections;

namespace Bikewale.Cache.Authors
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 20th Sep 2017
    /// </summary>
    public class AuthorsCacheRepository: IAuthorsCacheRepository
    {
        private readonly ICacheManager _cache;
        
        public AuthorsCacheRepository(ICacheManager cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Check valid author masking names
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        public AuthorsMaskingReponse GetAuthorsMaskingResponse(string maskingName)
        {
            AuthorsMaskingReponse response = new AuthorsMaskingReponse();
            Memcache.BWMemcache objMemcache = null;
            try
            {
                objMemcache = new Memcache.BWMemcache();
                var htNewMaskingNames = _cache.GetFromCache<Hashtable>("BW_AuthorMapping", new TimeSpan(24, 0, 0), () => objMemcache.GetHashTable("BW_AuthorMapping"));

                if (htNewMaskingNames != null && htNewMaskingNames.Contains(maskingName))
                {
                    response.AuthorId = Convert.ToInt32(htNewMaskingNames[maskingName]);
                }

                if (response.AuthorId > 0)
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 200;
                    return response;
                }
                else
                {
                    response.MaskingName = maskingName;
                    response.StatusCode = 404;
                    return response;
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetMakeMaskingResponse({0})", maskingName));
                
            }
            return response;
        } 
    }
}
