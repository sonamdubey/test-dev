
using BikewaleOpr.Entities.BikeData;
using System;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Aug 2017
    /// Modified by : Rajan Chauhan on 12th Dec 2017
    /// Summary : Added bike bodystyle field
    /// </summary>
    public class BikeSeriesEntity: BikeSeriesEntityBase
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsInActive { get; set; }
        public BikeMakeEntityBase BikeMake { get; set; }
        public BikeBodyStyleEntity BodyStyle { get; set; }
        public bool IsSeriesPageUrl { get; set; }
    }
}
