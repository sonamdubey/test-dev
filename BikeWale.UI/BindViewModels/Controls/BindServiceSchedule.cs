
using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by: Sangram Nandkhile
    /// Summary: To Bind bikes and service schedule details
    /// </summary>
    public class BindServiceSchedule
    {
        public int MakeId { get; set; }
        public uint CityId { get; set; }

        public IEnumerable<ModelServiceSchedule> GetServiceScheduleList(uint makeId)
        {
            IEnumerable<ModelServiceSchedule> bikeScheduleList = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                .RegisterType<ICacheManager, MemcacheManager>();
                var objSC = container.Resolve<IServiceCenter>();
                if (objSC != null)
                    bikeScheduleList = objSC.GetServiceScheduleByMake(makeId);
            }
            return bikeScheduleList;
        }
    }
}