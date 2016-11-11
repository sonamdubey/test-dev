
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using System.Collections.Generic;
using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;
namespace Bikewale.Interfaces.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// Modified by Sajal Gupta on 09-11-2016 added GetServiceCentersByCity and GetServiceCenterDataById method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IServiceCenterRepository<T, U> : IRepository<T, U>
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeId);
        IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId);
        ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId);
        IEnumerable<ModelServiceSchedule> GetServiceScheduleByMake(int makeId);
        ServiceCenterCompleteData GetServiceCenterDataById(uint serviceCenterId);
    }
}
