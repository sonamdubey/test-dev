using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikePricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By : Ashutosh Sharma on 31-07-2017
    /// Discription : ViewModel for Price monitoring report pgae. 
    /// </summary>
    public class PriceMonitoringVM
    {
        public PriceMonitoringEntity PriceMonitoringEntity { get; set; }

        public IEnumerable<BikeMakeEntityBase> BikeMakes { get; set; }

        public IEnumerable<Entities.StateEntityBase> States { get; set; }

        public uint StateId { get; set; }
        public uint MakeId { get; set; }

        public uint ModelId { get; set;}
    }
}
