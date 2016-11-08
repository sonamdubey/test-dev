using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenters;
using Bikewale.Notifications;
using System;

namespace Bikewale.Cache.ServiceCenters
{
    /// <summary>
    /// Created By : Sajal Gupta on 07/11/2016
    /// Description: Cache layer for Function for fetching service center data from cache.
    /// </summary>
    public class ServiceCentersCacheRepository : IServiceCentersCacheRepository
    {
        private readonly ICacheManager _cache = null;
        private readonly IServiceCentersRepository _obServiceCentersRepository = null;

        public ServiceCentersCacheRepository(ICacheManager cache, IServiceCentersRepository obServiceCentersRepository)
        {
            _cache = cache;
            _obServiceCentersRepository = obServiceCentersRepository;
        }

        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData objServiceCenterData = new ServiceCenterData();

            string key = String.Format("ServiceCenterList_{0}_{1}", cityId, makeId);
            try
            {
                objServiceCenterData = _cache.GetFromCache<ServiceCenterData>(key, new TimeSpan(1, 0, 0, 0), () => _obServiceCentersRepository.GetServiceCentersByCity(cityId, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersCacheRepository.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return objServiceCenterData;
        }
    }
}
