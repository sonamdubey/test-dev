using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    public class DealerPriceQuoteEntity
    {
        public IEnumerable<PQ_Price> PriceList { get; set; }
        public IEnumerable<OfferEntityBase> OfferList { get; set; }
        public IEnumerable<DealerQuotation> DealerDetails { get; set; }
        public IEnumerable<BikeColorAvailability> BikeAvailabilityByColor { get; set; }
    }
}
