
using Bikewale.Entities.service;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using System;
namespace Bikewale.Cache.ServiceCenter
{
    public class ServiceCenterCacheRepository : IServiceCenterCacheRepository
    {
        private readonly IServiceCenterRepository<ServiceCenterLocatorList, int> _objServiceCenter = null;
        private readonly ICacheManager _cache = null;

        public ServiceCenterCacheRepository(IServiceCenterRepository<ServiceCenterLocatorList, int> objServiceCenter, ICacheManager cache)
        {
            _objServiceCenter = objServiceCenter;
            _cache = cache;
        }

        public ServiceCenterLocatorList GetServiceCenterList(uint makeId)
        {

            ServiceCenterLocatorList objStateCityList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_ServiceCenter_{0}", makeId);
                objStateCityList = _cache.GetFromCache<ServiceCenterLocatorList>(key, new TimeSpan(1, 0, 0), () => _objServiceCenter.GetServiceCenterList(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CityCacheRepository.GetServiceCenterList");
                objErr.SendMail();
            }


            return objStateCityList;
        }
    }
}
