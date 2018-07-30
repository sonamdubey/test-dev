
using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Microsoft.Practices.Unity;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 09 Nov 2016
    /// Desc: Bind Service Center data
    /// </summary>
    public class BindServiceCenter
    {
        public int MakeId { get; set; }
        public uint CityId { get; set; }
        public ServiceCenterData serviceData = null;

        public ServiceCenterData GetServiceCenterList(int makeId, uint cityId)
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                .RegisterType<ICacheManager, MemcacheManager>();
                var objSC = container.Resolve<IServiceCenter>();
                if (objSC != null)
                    serviceData = objSC.GetServiceCentersByCity(cityId, makeId);
            }
            return serviceData;
        }
    }
}