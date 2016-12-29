using Bikewale.Entities.HomePage;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.HomePage;
using Bikewale.Notifications;
using System;

namespace Bikewale.Cache.HomePage
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   Home Page Banner Cache Repository
    /// </summary>
    public class HomePageBannerCacheRepository : IHomePageBannerCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IHomePageBannerRepository _homePageRepository;

        public HomePageBannerCacheRepository(ICacheManager cache, IHomePageBannerRepository homePageRepository)
        {
            _cache = cache;
            _homePageRepository = homePageRepository;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Gets Home Page Banner from Cache
        /// </summary>
        /// <returns></returns>
        public HomePageBannerEntity GetHomePageBanner()
        {
            string key = "BW_HomePageBanner";
            HomePageBannerEntity homePageBanner = null;
            try
            {
                homePageBanner = _cache.GetFromCache<HomePageBannerEntity>(key, new TimeSpan(0, 30, 0), () => _homePageRepository.GetHomePageBanner());

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "HomePageBannerCacheRepository.GetHomePageBanner");
            }
            return homePageBanner;
        }
    }
}
