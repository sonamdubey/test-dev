using BikewaleOpr.Entity.AdOperations;
using System.Collections.Generic;
namespace BikewaleOpr.Models.AdOperation
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Description: AdOperationVM 
    /// </summary>
    public class AdOperationVM
    {
        public IEnumerable<PromotedBike> PromotedBikeList { get; set; }
    }
}
