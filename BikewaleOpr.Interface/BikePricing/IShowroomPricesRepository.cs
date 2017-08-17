using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entities.BikePricing;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikePricing;

namespace BikewaleOpr.Interface.BikePricing
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
    }
}
