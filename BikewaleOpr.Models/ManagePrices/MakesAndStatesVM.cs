using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By : Prabhu Puredla on 22 May 2018
    /// Description : ViewModel which contains all makes and  states
    /// </summary>
    public class MakesAndStatesVM
    {
        public IEnumerable<BikeMakeEntityBase> BikeMakes { get; set; }
        public IEnumerable<BikewaleOpr.Entities.StateEntityBase> States { set; get; }
    }
}
