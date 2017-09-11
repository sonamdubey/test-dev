
using BikewaleOpr.Entities.BikeData;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Aug 2017
    /// </summary>
    public class BikeSeriesEntity: BikeSeriesEntityBase
    {
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public BikeMakeEntityBase BikeMake { get; set; }
    }
}
