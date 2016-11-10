
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using System;
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

        /// <summary>
        /// Created By : Sajal Gupta on 07/11/2016
        /// Description: BAL layer Function for fetching service center data from cache.
        /// </summary>
        public ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId)
        {
            ServiceCenterData objServiceCenterData = null;
            try
            {
                if (_objServiceCenter != null && cityId > 0 && makeId > 0)
                {
                    objServiceCenterData = _objServiceCenter.GetServiceCentersByCity(cityId, makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenters.GetServiceCentersByCity");
                objErr.SendMail();
            }
            return objServiceCenterData;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 09/11/2016
        /// Description: BAL layer Function for fetching service schedule from cache
        /// </summary>
        public IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(int makeId)
        {
            IEnumerable<ModelServiceSchedule> objServiceSchedule = null;
            try
            {
                if (_objServiceCenter != null && makeId > 0)
                {
                    objServiceSchedule = _objServiceCenter.GetServiceScheduleByMake(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenters.GetServiceScheduleByMake");
                objErr.SendMail();
            }
            return objServiceSchedule;
        }
    }
}
