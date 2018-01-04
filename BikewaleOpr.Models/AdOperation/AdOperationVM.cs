using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using System.Collections.Generic;
namespace BikewaleOpr.Models
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Description: AdOperationVM 
    /// </summary>
    public class AdOperationVM
    {
        public IEnumerable<PromotedBike> PromotedBikeList { get; set; }
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public uint UserId { get; set; }
    }
}
