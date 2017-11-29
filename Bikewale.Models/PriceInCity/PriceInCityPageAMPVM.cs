using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models.PriceInCity
{
    public class PriceInCityPageAMPVM : PriceInCityPageVM
    {
        public IEnumerable<BikeQuotationAMPEntity> FormatedBikeVersionPrices { get; set; }
        public Entities.EMISliderAMP EMISliderAMP { get; set; }
        public string JSONEMISlider { get; set; }
        public EMI EMI{ get; set; }
        public int TotalAmount { get; set; }

    }

}
