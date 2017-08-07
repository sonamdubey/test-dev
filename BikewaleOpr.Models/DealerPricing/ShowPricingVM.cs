using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class ShowPricingVM
    {
        public IEnumerable<CityNameEntity> Cities { get; set; }
    }
}
