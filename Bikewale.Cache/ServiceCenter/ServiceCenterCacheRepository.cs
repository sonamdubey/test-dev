
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
namespace Bikewale.Cache.ServiceCenter
{

    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class ServiceCenterCacheRepository : IServiceCenterCacheRepository
    {
        private readonly IServiceCenterRepository<ServiceCenterLocatorList, int> _objServiceCenter = null;
        private readonly ICacheManager _cache = null;

        public ServiceCenterCacheRepository(IServiceCenterRepository<ServiceCenterLocatorList, int> objServiceCenter, ICacheManager cache)
        {
            _objServiceCenter = objServiceCenter;
            _cache = cache;
        }


        /// <summary>
        /// Created by:-Subodh Jain 7 nov 2016
        /// Summary:- Get make wise list of service center in cities and state
        /// </summary>
        /// <param name="makeid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: Cache layer for Function for fetching service center data from cache.
        /// </summary>
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            string key = String.Format("BW_ServiceCenterList_{0}_{1}", cityId, makeId);
            try
            {
                return _cache.GetFromCache<ServiceCenterData>(key, new TimeSpan(1, 0, 0, 0), () => _objServiceCenter.GetServiceCentersByCity(cityId, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersCacheRepository.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return null;
        }

        public IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(uint makeId)
        {
            string key = String.Format("BW_ServiceScheduleByMake_{0}", makeId);
            try
            {
                return _cache.GetFromCache<IEnumerable<ModelServiceSchedule>>(key, new TimeSpan(1, 0, 0, 0), () => _objServiceCenter.GetServiceScheduleByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersCacheRepository.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return null;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: Cache layer for Function for fetching service center data from cache.
        /// </summary>
        public ServiceCenterCompleteData GetServiceCenterDataById(uint serviceCenterId)
        {
            string key = String.Format("BW_ServiceCenterData_{0}", serviceCenterId);
            try
            {
                return _cache.GetFromCache<ServiceCenterCompleteData>(key, new TimeSpan(1, 0, 0, 0), () => _objServiceCenter.GetServiceCenterDataById(serviceCenterId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Error in ServiceCentersCacheRepository.GetServiceCenterDataById for parameters serviceCenterId : {0}", serviceCenterId));
                objErr.SendMail();
            }
            return null;
        }
    }
}
