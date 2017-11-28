using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 15 Dec 2016
    /// Summary    : To bind service center data by brand
    /// Modified By:- Subodh Jain 20 Dec 2016
    /// Summary : Added try catch
    /// </summary>
    public class BindOtherBrandsServiceCenters
    {
        /// <summary>
        /// Created By : Aditi Srivastava on 15 Dec 2016
        /// Summary    : To bind service center data by brand
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BrandServiceCenters> GetAllServiceCentersbyMake()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    var objSC = container.Resolve<IServiceCenterCacheRepository>();
                    return objSC.GetAllServiceCentersByBrand();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetAllServiceCentersbyMake");
                
                return null;
            }
            
        }
    }
}