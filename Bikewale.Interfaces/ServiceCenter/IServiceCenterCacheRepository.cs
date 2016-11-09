
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using System.Collections.Generic;
using Bikewale.Entities.ServiceCenters;
namespace Bikewale.Interfaces.ServiceCenter
{
    /// <summary>
    /// Created By:-Subodh jain 7 nov 2016
    /// Summary:- For service center locator 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IServiceCenterCacheRepository
    {
        ServiceCenterLocatorList GetServiceCenterList(uint makeid);
        IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeid);
        ServiceCenterData GetServiceCentersByCity(uint cityId, int makeId);
    }
}
