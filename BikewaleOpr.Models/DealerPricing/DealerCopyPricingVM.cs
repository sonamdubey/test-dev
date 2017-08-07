using BikewaleOpr.Entity;
using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class DealerCopyPricingVM
    {
        public IEnumerable<CityNameEntity> Cities { get; set; }
        public IEnumerable<DealerMakeEntity> Dealers { get; set; }
    }
}
