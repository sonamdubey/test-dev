using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikeData
{
    /// <summary>
    /// to store list of models grouped by make.
    /// 
    /// created by: vivek singh tomar on 27/07/2017
    /// </summary>
    public class BikeModelsByMake
    {
        public IEnumerable<BikeModelEntityBase> BikeModelEntity { get; set; }
        public BikeMakeEntityBase BikeMakeEntity { get; set; }
    }
}
