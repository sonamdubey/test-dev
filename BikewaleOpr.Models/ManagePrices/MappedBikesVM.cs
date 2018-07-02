using BikewaleOpr.Entity.BikePricing;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ManagePrices
{

    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : ViewModel for mapped bikes page of Bulk Price upload. 
    /// </summary>
    public class MappedBikesVM
    {
        public IEnumerable<MappedBikesEntity> MappedBikes { set; get; }
        public MakesAndStatesVM MakesListVM { set; get; }
        public uint MakeId { set; get; }
        public string MakeName { set; get; }
    }
}
