using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details cache repository for used bikes section
    /// </summary>
    public class UsedBikeDetailsCache : IUsedBikeDetailsCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IUsedBikeDetails _objUsedBikes;

        /// <summary>
        /// Intitalize the references for the cache and DL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objUsedBikes"></param>
        public UsedBikeDetailsCache(ICacheManager cache, IUsedBikeDetails objUsedBikes)
        {
            _cache = cache;
            _objUsedBikes = objUsedBikes;
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Cache layer profile details for the used bike
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetails GetProfileDetails(uint inquiryId)
        {
            ClassifiedInquiryDetails objUsedBikes = null;
            string key = String.Format("BW_ProfileDetails_{0}", inquiryId);
            try
            {
                objUsedBikes = _cache.GetFromCache<ClassifiedInquiryDetails>(key, new TimeSpan(0, 30, 0), () => _objUsedBikes.GetProfileDetails(inquiryId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Cache.Used.GetProfileDetails");
                objErr.SendMail();
            }
            return objUsedBikes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetBikesByCityId(uint inquiryId, uint cityId)
        {
            throw new NotImplementedException();
        }
    }
}
