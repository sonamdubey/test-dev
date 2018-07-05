using Bikewale.Entities.BikeBooking;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created by  : Pratibha Verma on 19 JUne 2018
    /// Description : used new entity for PQOutputEntity
    /// </summary>
    public class PQOnRoadPrice
    {
        public Bikewale.Entities.BikeBooking.v2.PQOutputEntity PriceQuote { get; set; }
        public PQ_QuotationEntity DPQOutput { get; set; }
        public BikeQuotationEntity BPQOutput { get; set; }
        public bool IsDealerPriceAvailable { get { if (this.DPQOutput != null) { return true; } else { return false; } } }
        public bool IsInsuranceFree { get; set; }
        public uint InsuranceAmount { get; set; }
        public List<PQ_Price> discountedPriceList { get; set; }
        public bool IsDiscount { get; set; }
        public uint BaseVersion { get; set; }
    }
}
