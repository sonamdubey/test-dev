using BikewaleOpr.Entity.BikePricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BikewaleOpr.Interface.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : Provides BAL methods for bulk price upload
    /// </summary>
    public interface IBulkPrice
    {
        CompositeBulkPriceEntity GetProcessedData(uint makeId, XmlReader xml);
        bool SavePrices(IEnumerable<OemPriceEntity> PricesList, uint updatedBy);
        bool MapUnmappedBike(string oemBikeName, uint bikeId, IEnumerable<OemPriceEntity> unmappedOemPricesList, ICollection<OemPriceEntity> updatedPriceList, ICollection<string> unmappedBikes, uint updatedBy);
        bool MapUnmappedCity(string oemCityName, uint cityId, IEnumerable<OemPriceEntity> unmappedOemPricesList, ICollection<OemPriceEntity> updatedPriceList, ICollection<string> unmappedCities, uint updatedBy);
    }
}
