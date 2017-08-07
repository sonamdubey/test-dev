using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class CityCopyPricingVM
    {
        public IEnumerable<StateEntityBase> States { get; set; }
        public IEnumerable<CityNameEntity> Cities { get; set; }
    }
}
