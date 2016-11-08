using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenters;
using Bikewale.Notifications;
using System;

namespace Bikewale.BAL.ServiceCenters
{
    /// <summary>
    /// Created By : Sajal Gupta on 07/11/2016
    /// Description: BAL layer for fetching service center data.
    /// </summary>
    public class ServiceCenters : IServiceCenters
    {
        private readonly ICacheManager _cache = null;
        private readonly IServiceCentersRepository _obServiceCentersRepository = null;
        private readonly IServiceCentersCacheRepository _obServiceCentersCacheRepository = null;

        public ServiceCenters(ICacheManager cache, IServiceCentersRepository obServiceCentersRepository, IServiceCentersCacheRepository obServiceCentersCacheRepository)
        {
            _cache = cache;
            _obServiceCentersRepository = obServiceCentersRepository;
            _obServiceCentersCacheRepository = obServiceCentersCacheRepository;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: BAL layer Function for fetching service center data from cache.
        /// </summary>
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData objServiceCenterData = null;
            try
            {
                if (_obServiceCentersCacheRepository != null && cityId > 0 && makeId > 0)
                {
                    objServiceCenterData = _obServiceCentersCacheRepository.GetServiceCentersByCity(cityId, makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenters.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return objServiceCenterData;
        }
    }
}
