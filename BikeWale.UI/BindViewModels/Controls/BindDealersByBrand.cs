

using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 15 Dec 2016
    /// Summary    : To bind service center data by brand
    /// </summary>
    public class BindDealersByBrand
    {
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public int Count { get; set; }
        public IEnumerable<DealerBrandEntity> serviceData = null;
        public IEnumerable<DealerBrandEntity> GetAllServiceCentersbyMake()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>()
                           .RegisterType<IDealer, DealersRepository>()
                          ;
                var objCache = container.Resolve<IDealerCacheRepository>();

                if (objCache != null)
                    serviceData = objCache.GetDealerByBrandList();
            }
            return serviceData;
        }

    }
}