
using Bikewale.Entities.Location;
using Bikewale.Entities.MobileVerification;
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
        private readonly IServiceCenterRepository<ServiceCenterLocatorList, int> _objSMSData = null;

        public ServiceCenter(IServiceCenterCacheRepository ObjServiceCenter, IServiceCenterRepository<ServiceCenterLocatorList, int> ObjSMSData)
        {
            _objServiceCenter = ObjServiceCenter;
            _objSMSData = ObjSMSData;
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
                ErrorClass.LogError(ex, "ServiceCenters.GetServiceCentersByCity");
                
            }
            return objServiceCenterData;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 09/11/2016
        /// Description: BAL layer Function for fetching service schedule from cache
        /// </summary>
        public IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(uint makeId)
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
                ErrorClass.LogError(ex, "ServiceCenters.GetServiceScheduleByMake");
                
            }
            return objServiceSchedule;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 09/11/2016
        /// Description: BAL layer Function for fetching service center complete data from cache.
        /// </summary>
        public ServiceCenterCompleteData GetServiceCenterDataById(uint serviceCenterId)
        {
            try
            {
                if (_objServiceCenter != null && serviceCenterId > 0)
                {
                    return _objServiceCenter.GetServiceCenterDataById(serviceCenterId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCenters.GetServiceCenterDataById for parameters serviceCenterId : {0}", serviceCenterId));
                
            }
            return null;
        }
        /// <summary>
        /// Created by:-Subodh Jain 7 nov 2016
        /// Summary:- Get make wise list of cities for service center
        /// </summary>
        /// <param name="makeid"></param>
        /// <returns></returns>
        public IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeid)
        {
            return _objServiceCenter.GetServiceCenterCities(makeid);
        }

        /// <summary>
        /// Created By : Sajal Gupta on 16/11/2016
        /// Description: BAL layer Function for sending service center sms data from DAL.
        /// </summary>
        public EnumSMSStatus GetServiceCenterSMSData(uint serviceCenterId, string mobileNumber, string pageUrl)
        {
            try
            {
                SMSData objSMSData = _objSMSData.GetServiceCenterSMSData(serviceCenterId, mobileNumber);

                if (objSMSData != null)
                {
                    if (objSMSData.SMSStatus == EnumSMSStatus.Success)
                    {
                        SMSTypes newSms = new SMSTypes();
                        newSms.ServiceCenterDetailsSMS(mobileNumber, objSMSData.Name, objSMSData.Address, objSMSData.Phone, objSMSData.CityName, pageUrl);
                        return EnumSMSStatus.Success;
                    }
                    else if (objSMSData.SMSStatus == EnumSMSStatus.Daily_Limit_Exceeded)
                    {
                        return EnumSMSStatus.Daily_Limit_Exceeded;
                    }
                    else
                    {
                        return EnumSMSStatus.Invalid;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Error in ServiceCenters.GetServiceCenterSMSData for parameters serviceCenterId : {0}, mobileNumber : {1}", serviceCenterId, mobileNumber));
                
            }
            return 0;
        }
        
    }
}
