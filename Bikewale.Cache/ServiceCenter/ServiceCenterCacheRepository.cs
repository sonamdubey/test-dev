
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
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
                key = String.Format("BW_ServiceCenterCityState_{0}", makeId);
                objStateCityList = _cache.GetFromCache<ServiceCenterLocatorList>(key, new TimeSpan(1, 0, 0), () => _objServiceCenter.GetServiceCenterList(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CityCacheRepository.GetServiceCenterList");
                objErr.SendMail();
            }


            return objStateCityList;
        }
        public IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId)
        {

            IEnumerable<CityEntityBase> objStateCityList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_ServiceCenterCity_{0}", makeId);
                objStateCityList = _cache.GetFromCache<IEnumerable<CityEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objServiceCenter.GetServiceCenterCities(makeId));
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
