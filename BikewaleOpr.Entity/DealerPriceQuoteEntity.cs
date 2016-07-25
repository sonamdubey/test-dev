using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Entities
{
    public class DealerPriceQuoteEntity
    {
        public IEnumerable<PQ_Price> PriceList { get; set; }
        public IEnumerable<OfferEntityBase> OfferList { get; set; }
        public IEnumerable<DealerQuotation> DealerDetails { get; set; }
        public IEnumerable<BikeColorAvailability> BikeAvailabilityByColor { get; set; }
    }
}
