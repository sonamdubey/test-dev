using BikewaleOpr.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Models.ManagePrices
{
    public class BikesMakeListVM
    {
        public IEnumerable<BikeMakeEntityBase> BikeMakes { get; set; }
    }
}
