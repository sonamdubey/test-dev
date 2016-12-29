using Bikewale.Cache.Core;
using Bikewale.Cache.HomePage;
using Bikewale.DAL.HomePage;
using Bikewale.Entities.HomePage;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.HomePage;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
namespace Bikewale.BindViewModels.Webforms
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   Bind Default Page
    /// </summary>
    public class BindDefaultPage
    {
        public HomePageBannerEntity HomePage { get; set; }
        /// <summary>
        /// Constructor to get the Home Page Banner data
        /// </summary>
        public BindDefaultPage()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IHomePageBannerRepository, HomePageBannerRepository>()
                        .RegisterType<IHomePageBannerCacheRepository, HomePageBannerCacheRepository>();
                    IHomePageBannerCacheRepository cache = container.Resolve<IHomePageBannerCacheRepository>();

                    HomePage = cache.GetHomePageBanner();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindDefaultPage.Constructor");
            }
        }
    }
}