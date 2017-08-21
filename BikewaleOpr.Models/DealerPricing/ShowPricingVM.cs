using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for Show Pricing collapsable in dealer pricing management page
    /// </summary>
    public class ShowPricingVM
    {
        public IEnumerable<CityNameEntity> Cities { get; set; }
    }
}
