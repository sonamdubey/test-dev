using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entity.ServiceCenter;
using BikewaleOpr.Entity;

namespace BikewaleOpr.Interface.ServiceCenter
{
    /// <summary>
    /// Created BY : Snehal Dange on 28 July 2017
    /// Summary : Interface methods related to service center BAL
    /// </summary>
    public interface IServiceCenter
    {
       IEnumerable<Entities.BikeData.BikeMakeEntityBase> GetBikeMakes(ushort requestType);
       IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId);

       IEnumerable<CityEntityBase> GetAllCities();
        ServiceCenterData GetServiceCentersByCityMake(uint cityId, uint makeId ,sbyte activeStatus);

        ServiceCenterCompleteData GetServiceCenterDetailsbyId(uint serviceCenterId);

        StateCityEntity GetStateDetailsByCity(uint cityId);

        bool AddUpdateServiceCenter(ServiceCenterCompleteData serviceCenterDetails, string _updatedBy);
        bool UpdateServiceCenterStatus(uint serviceCenterId, string currentUserId);
    }
}
