using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for pricing table in dealer pricing management page
    /// </summary>
    public class DealerPriceSheetVM
    {
        public IEnumerable<DealerVersionPriceEntity> dealerVersionPricings { get; set; }
    }
}
