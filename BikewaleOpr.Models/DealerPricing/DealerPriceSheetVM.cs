using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class DealerPriceSheetVM
    {
        public IEnumerable<DealerVersionPriceEntity> dealerVersionPricings { get; set; }
    }
}
