
using Bikewale.Entities.HomePage;
namespace Bikewale.Interfaces.HomePage
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   Interface for HomePageBannerCacheRepository
    /// </summary>
    public interface IHomePageBannerCacheRepository
    {
        HomePageBannerEntity GetHomePageBanner(uint platformId);
    }
}
