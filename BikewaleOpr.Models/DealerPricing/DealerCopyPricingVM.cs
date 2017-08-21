using BikewaleOpr.Entity;
using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for copy pricing to other dealers collapsable in dealer pricing management page
    /// </summary>
    public class DealerCopyPricingVM
    {
        public IEnumerable<CityNameEntity> Cities { get; set; }
        public IEnumerable<DealerMakeEntity> Dealers { get; set; }
    }
}
