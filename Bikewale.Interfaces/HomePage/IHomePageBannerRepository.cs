
using Bikewale.Entities.HomePage;
using System;
namespace Bikewale.Interfaces.HomePage
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   Interface for HomePageBannerRepository
    /// </summary>
    public interface IHomePageBannerRepository
    {
        Tuple<HomePageBannerEntity, TimeSpan> GetHomePageBannerWithCacheTime(uint platformId);
    }
}
