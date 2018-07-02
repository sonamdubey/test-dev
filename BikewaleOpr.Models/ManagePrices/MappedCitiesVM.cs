using BikewaleOpr.Entity.BikePricing;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : ViewModel for mapped cities page of Bulk Price upload. 
    /// </summary>
    public class MappedCitiesVM
    {
        public IEnumerable<MappedCitiesEntity> MappedCities { set; get; }
        public MakesAndStatesVM MakesListVM { set; get; }
        public uint StateId { set; get; }
        public string StateName { set; get; }
    }
}
