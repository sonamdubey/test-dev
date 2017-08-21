using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for dealer operation collapsable in dealer pricing management index page
    /// </summary>
    public class DealerOperationVM
    {
        public IEnumerable<CityNameEntity> DealerCities { get; set; }
        public bool IsOpen { get; set; }
    }
}
