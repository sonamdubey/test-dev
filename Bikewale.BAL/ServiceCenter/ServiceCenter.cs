
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using Bikewale.Interfaces.ServiceCenter;
using System.Collections.Generic;
namespace Bikewale.BAL.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class ServiceCenter<T, U> : IServiceCenter where T : ServiceCenterLocatorList, new()
    {
        private readonly IServiceCenterCacheRepository _objServiceCenter = null;
        public ServiceCenter(IServiceCenterCacheRepository ObjServiceCenter)
        {
            _objServiceCenter = ObjServiceCenter;


        }
        /// <summary>
        /// Created by:-Subodh Jain 7 nov 2016
        /// Summary:- Get make wise list of service center in cities and state
        /// </summary>
        /// <param name="makeid"></param>
        /// <returns></returns>
        public ServiceCenterLocatorList GetServiceCenterList(uint makeid)
        {
            return _objServiceCenter.GetServiceCenterList(makeid);
        }
        public IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeid)
        {
            return _objServiceCenter.GetServiceCenterCities(makeid);
        }

    }
}
