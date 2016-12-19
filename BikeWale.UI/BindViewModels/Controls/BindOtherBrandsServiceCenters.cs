using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 15 Dec 2016
    /// Summary    : To bind service center data by brand
    /// </summary>
    public class BindOtherBrandsServiceCenters
    {
        public string MakeName{ get; set; }
        public string MakeMaskingName { get; set; }
        public int Count { get; set; }
        public IEnumerable<BrandServiceCenters> serviceData = null;
        public IEnumerable<BrandServiceCenters> GetAllServiceCentersbyMake()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                .RegisterType<ICacheManager, MemcacheManager>();
                var objSC = container.Resolve<IServiceCenter>();
                if (objSC != null)
                    serviceData = objSC.GetAllServiceCentersByBrand();
            }
            return serviceData;
        }
    }
}