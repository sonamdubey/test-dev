using System.Collections.Generic;
using BikewaleOpr.Entity.BikePricing;

namespace BikewaleOpr.Interface.BikePricing
{
    /// <summary>
    /// Created by : Prabhu Puredla on 18 May 2018
    /// Description : Provide BAL methods for Bikewale Bulk Price.
    /// </summary>
    public interface IBulkPriceRepository
    {
        IEnumerable<MappedBikesEntity> GetMappedBikesData(uint makeId);
        void DeleteMappedBike(uint mappingId,uint updatedBy);
        IEnumerable<MappedCitiesEntity> GetMappedCitiesData(uint stateId);
        void DeleteMappedCity(uint mappingId, uint updatedBy);    
        IEnumerable<MappedCitiesEntity> GetAllMappedCitiesData();
        bool MapTheUnmappedBike(string oemBikeName, uint versionId, uint updatedBy);
        bool MapTheUnmappedCity(uint cityId, string oemCityName, uint updatedBy);
        IEnumerable<MappedBikesEntity> GetAllMappedBikesData();
        bool SavePrices(string pricesToUpdate, uint bikeId, uint updatedBy);
    }
}
