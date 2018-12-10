using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces.Advertizings.App;
using Carwale.Entity.Advertizings.Apps;
using Carwale.Interfaces;
using Carwale.DAL.Advertizings.App;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.Advertizing.App
{
    public class SplashScreenCache : IAppSplashCache
    {
        private readonly IAppSplashRepository _splashScreen;
        private readonly ICacheManager _cacheProvider;

        public SplashScreenCache(IAppSplashRepository splashScreen, ICacheManager cacheProvider)
        {
            _splashScreen = splashScreen;
            _cacheProvider = cacheProvider;
        }

        public List<SplashScreenBanner> GetSplashSreenBanner(int platformId, int applicationId)
        {
            return _cacheProvider.GetFromCache<List<SplashScreenBanner>>(string.Format("splashscr_v2_{0}_{1}",applicationId, platformId), new TimeSpan(0, 30, 0), 
                () => _splashScreen.GetSpalshSreenBanner(platformId, applicationId));
        }

        public void DeactivaSplashBanner(int platformId, int applicationId)
        {
            _cacheProvider.ExpireCache(string.Format("splashscr_v1_{0}_{1}", applicationId, platformId));
        }
    }
}
