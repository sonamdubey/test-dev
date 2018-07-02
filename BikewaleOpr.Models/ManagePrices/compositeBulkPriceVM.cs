using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikePricing;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ManagePrices
{

    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : ViewModel which contains processed data of uploaded prices file.
    /// </summary>
    public class CompositeBulkPriceVM
    {
        public ICollection<string> UnmappedCities { set; get; }
        public ICollection<string> UnmappedBikes { set; get; }
        public IEnumerable<BikeModelEntityBase> BikeModelList { get; set; }
        public IEnumerable<BikewaleOpr.Entities.StateEntityBase> States { set; get; }
        public ICollection<OemPriceEntity> UpdatedPriceList { set; get; }
        public IEnumerable<OemPriceEntity> UnmappedOemPricesList { set; get; }
    }
}
