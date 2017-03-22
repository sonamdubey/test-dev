using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entities.BikeData
{
    public class BikeMakeEntity : BikeMakeEntityBase
    {
        public bool Futuristic { get; set; }
        public bool New { get; set; }
        public bool Used { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
