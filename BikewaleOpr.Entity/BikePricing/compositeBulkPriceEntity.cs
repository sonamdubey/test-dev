using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : Entity for holding processed data of price file
    /// </summary>
    public class CompositeBulkPriceEntity
    {
        public ICollection<string> UnmappedCities { set; get; }
        public ICollection<string> UnmappedBikes { set; get; }
        public IEnumerable<BikeModelEntityBase> BikeModelList { get; set; }
        public IEnumerable<BikewaleOpr.Entities.StateEntityBase> States { set; get; }
        public ICollection<OemPriceEntity> UpdatedPriceList { set; get; }
        public ICollection<OemPriceEntity> UnmappedOemPricesList { set; get; }
        public ICollection<OemPriceEntity> OemPricesList { set; get; }
        public IDictionary<string, uint> CitiesTable { set; get; }
        public IDictionary<string, uint> StatesTable { set; get; }
        public IDictionary<string, uint> BikesTable { set; get; }
        public IDictionary<string, uint> ModelsTable { set; get; }
    }
}
