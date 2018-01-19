
using Bikewale.Entities.Location;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;
namespace Bikewale.Interfaces.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// Modified by Sajal Gupta on 09-11-2016 added GetServiceCentersByCity and GetServiceCenterDataById method.
    /// Modified by Sajal Gupta on 16-11-2016 added GetServiceCenterSMSData method.
    /// Modified by : Aditi Srivastava on 15 Dec 2016 
    /// Summary : Added function to get service centers by make
    /// Modified by : Aditi Srivastava on 19 Dec 2016 
    /// Summary : Added function to get service centers by make in nearby cities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IServiceCenterRepository<T, U> : IRepository<T, U>
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeId);
        IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId);
        ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId);
        IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(uint makeId);
        ServiceCenterCompleteData GetServiceCenterDataById(uint serviceCenterId);
        SMSData GetServiceCenterSMSData(uint serviceCenterId, string mobileNumber);
        IEnumerable<BrandServiceCenters> GetAllServiceCentersByBrand();
        IEnumerable<CityBrandServiceCenters> GetServiceCentersNearbyCitiesByBrand(int cityId, int makeId, int topCount);
    }
}
