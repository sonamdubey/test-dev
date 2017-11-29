using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.MobileVerification
{
    /// <summary>
    /// Created by : Aditi Srivastava on 14 Feb 2017
    /// Summary    : To cache the list of verified phone numbers
    /// </summary>
    public class MobileVerificationCache : IMobileVerificationCache
    {
        private readonly ICacheManager _cache = null;
        private readonly IMobileVerificationRepository _objMobileVer = null;
        public MobileVerificationCache(ICacheManager objCache, IMobileVerificationRepository objMobileVer)
        {
            _cache = objCache;
            _objMobileVer = objMobileVer;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 14 Feb 2017
        /// Summary    : Get list of blocked mobile numbers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetBlockedNumbers()
        {
            IEnumerable<string> numberList = null;
            string key = "BW_BlockedMobileNumbers";
            try
            {
                numberList = _cache.GetFromCache<IEnumerable<string>>(key, new TimeSpan(2, 0, 0), () => _objMobileVer.GetBlockedPhoneNumbers());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.MobileVerification.GetVerifiedNumbers");
            }
            return numberList;
        }
    }
}
