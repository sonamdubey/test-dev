
using BikewaleOpr.Entities.BikeData;
using System;
namespace BikewaleOpr.Entity.AdOperations
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Description: Entity created for promoted bike 
    /// </summary>
    public class PromotedBike
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AdOperationEnum AdOperationType { get; set; }
        public bool IsActive { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

    }
}
