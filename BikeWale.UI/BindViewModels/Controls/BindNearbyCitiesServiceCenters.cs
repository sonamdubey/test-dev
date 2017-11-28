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
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 15 Dec 2016
    /// Summary    : To bind service center data of nearby cities of a brand
    /// </summary>
    public class BindNearbyCitiesServiceCenters
    {
        public IEnumerable<CityBrandServiceCenters> GetServiceCentersNearbyCitiesByMake(int cityId,int makeId,int topCount)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    var objSC = container.Resolve<IServiceCenterCacheRepository>();
                   return objSC.GetServiceCentersNearbyCitiesByBrand(cityId, makeId, topCount);
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Error in BindNearbyCitiesServiceCenters.GetServiceCentersNearbyCitiesByMake");
                
                return null;
            }
           
            
        }
    }
}