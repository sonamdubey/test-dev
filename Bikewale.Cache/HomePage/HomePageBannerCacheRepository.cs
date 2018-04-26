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
        /// Modified By:-Subodh Jain 26 july 2017
        /// Summary :- changed cache key and modified according to platform id
        /// Modified By : Rajan Chauhan on 26 Apr 2018
        /// Description : Changed cache key version from BW_HomePageBanner_PlatformId_ 
        ///               to BW_HomePageBanner_V1_PlatformId_
        /// </summary>
        /// <returns></returns>
        public HomePageBannerEntity GetHomePageBanner(uint platformId)
        {
            string key = string.Format("BW_HomePageBanner_V1_PlatformId_{0}", platformId);
            HomePageBannerEntity homePageBanner = null;
            try
            {
                homePageBanner = _cache.GetFromCache<HomePageBannerEntity>(key, () => _homePageRepository.GetHomePageBannerWithCacheTime(platformId));

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("HomePageBannerRepository.GetHomePageBanner platformid:{0}", platformId));
            }
            return homePageBanner;
        }
    }
}
