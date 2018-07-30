
using Bikewale.Entities.Location;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;
namespace Bikewale.Interfaces.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// Modified by Sajal Gupta on 09-11-2016 added GetServiceCentersByCity and GetServiceCenterDataById method.
    /// Modified by Sajal Gupta on 16-11-2016 added GetServiceCenterSMSData method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IServiceCenter
    {

        IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId);
        ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId);
        IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(uint makeId);
        ServiceCenterCompleteData GetServiceCenterDataById(uint serviceCenterId);
        EnumSMSStatus GetServiceCenterSMSData(uint serviceCenterId, string mobileNumber, string pageUrl);
     }
}
