using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Entity.BikeData
{
    public class UsedModelsByMake : BikeMakeEntityBase
    {
        public IEnumerable<string> ModelList { get; set; }
    }
}
