using Bikewale.ElasticSearch.Entities;
using BikewaleOpr.Entities.BikePricing;
using BikewaleOpr.Entity.BikePricing;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.Dealers
{
    /// <summary>
    /// Modified By : Ashutosh Sharma on 31-07-2017
    /// Discription : Added GetPriceMonitoringDetails
    /// </summary>
    public interface IShowroomPricesRepository
    {
        IEnumerable<BikePrice> GetBikePrices(uint makeId, uint cityId);
        bool SaveBikePrices(string versionPriceList, string citiesList, int updatedBy);
        PriceMonitoringEntity GetPriceMonitoringDetails(uint makeId, uint modelId, uint stateId);
        ModelPriceDocument GetModelPriceDocument(uint makeId, uint cityId);
    }
}
