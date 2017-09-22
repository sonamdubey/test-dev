using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.ServiceCenters;

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
