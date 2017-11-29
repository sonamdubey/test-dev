
using Bikewale.Entities;
using Bikewale.Interfaces.App;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
namespace Bikewale.Cache.App
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 05-May-2017
    /// Cache for splash screens
    /// </summary>
    public class SplashScreenCacheRepository : ISplashScreenCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly ISplashScreenRepository _splashScreen;

        public SplashScreenCacheRepository(ICacheManager cache, ISplashScreenRepository splashScreen)
        {
            _cache = cache;
            _splashScreen = splashScreen;
        }

        /// <summary>
        /// Gets the application splash screen.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 08-May-2017 
        /// </returns>
        public IEnumerable<SplashScreenEntity> GetAppSplashScreen()
        {
            IEnumerable<SplashScreenEntity> objCacheScreens = null;

            string key = String.Format("BW_App_SplashScreens_Images");
            try
            {
                objCacheScreens = _cache.GetFromCache<IEnumerable<SplashScreenEntity>>(key, new TimeSpan(24, 0, 0), () => _splashScreen.GetAppSplashScreen());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SplashScreenCacheRepository.GetAppSplashScreen");
            }
            return objCacheScreens;
        }
    }
}
