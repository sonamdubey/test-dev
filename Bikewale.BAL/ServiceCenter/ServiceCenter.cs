
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web;
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
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenters.GetServiceCentersByCity");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenters.GetServiceScheduleByMake");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("Error in ServiceCenters.GetServiceCenterDataById for parameters serviceCenterId : {0}", serviceCenterId));
                objErr.SendMail();
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
        public int GetServiceCenterSMSData(uint serviceCenterId, string mobileNumber, string pageUrl)
        {
            try
            {
                ServiceCenterSMSData objSMSData = _objSMSData.GetServiceCenterSMSData(serviceCenterId, mobileNumber);

                if (objSMSData != null)
                {
                    if (objSMSData.SMSStatus == EnumServiceCenterSMSStatus.Success)
                    {
                        SMSTypes newSms = new SMSTypes();
                        newSms.ServiceCenterDetailsSMS(mobileNumber, objSMSData.Name, objSMSData.Address, objSMSData.Phone, objSMSData.CityName, pageUrl);
                        return (int)EnumServiceCenterSMSStatus.Success;
                    }
                    else if (objSMSData.SMSStatus == EnumServiceCenterSMSStatus.Daily_Limit_Exceeded)
                    {
                        return (int)EnumServiceCenterSMSStatus.Daily_Limit_Exceeded;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Error in ServiceCenters.GetServiceCenterSMSData for parameters serviceCenterId : {0}, mobileNumber : {1}", serviceCenterId, mobileNumber));
                objErr.SendMail();
            }
            return 0;
        }

    }
}
