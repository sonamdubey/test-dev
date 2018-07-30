

using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Subodh Jain on 20 Dec 2016
    /// Summary    : To bind dealers data by brand
    /// </summary>
    public class BindDealersByBrand
    {
        /// <summary>
        /// Created By :  Subodh Jain on 20 Dec 2016
        /// Summary    :  To bind dealers data by brand
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DealerBrandEntity> GetDealerByBrandList()
        {
            IEnumerable<DealerBrandEntity> dealersData = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                               .RegisterType<ICacheManager, MemcacheManager>()
                               .RegisterType<IDealerRepository, DealersRepository>()
                              ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    dealersData = objCache.GetDealerByBrandList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindDealersByBrand.GetDealerByBrandList()");
                
            }
            return dealersData;
        }

    }
}