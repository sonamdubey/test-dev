using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ServiceCenter
{
    /// <summary>
    /// Created BY : Snehal Dange on 28 July 2017
    /// Summary : Interface methods related to service center
    /// </summary>
    public interface IServiceCenterRepository
    {
        IEnumerable<CityEntityBase> GetServiceCenterCities(uint makeId);
        IEnumerable<CityEntityBase> GetAllCities();
        ServiceCenterData GetServiceCentersByCityMake(uint cityId, uint makeId , sbyte activeStatus);
        StateCityEntity GetStateDetails(uint cityId);

        ServiceCenterCompleteData GetDataById(uint serviceCenterId);
        bool AddUpdateServiceCenter(ServiceCenterCompleteData serviceCenterDetails, string updatedBy);

        bool UpdateServiceCenterStatus(uint serviceCenterId, string updatedBy);

    }
}
