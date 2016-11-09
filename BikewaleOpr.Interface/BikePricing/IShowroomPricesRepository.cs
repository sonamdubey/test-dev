using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entities.BikePricing;

namespace BikewaleOpr.Interface.BikePricing
{
    public interface IShowroomPricesRepository
    {
        IEnumerable<BikePrice> GetBikePrices(uint makeId, uint cityId);
        bool SaveBikePrices(string versionPriceList, string citiesList, int updatedBy);
    }
}
