using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    public class PQOnRoadPrice
    {
        public PQOutputEntity PriceQuote { get; set; }
        public PQ_QuotationEntity DPQOutput { get; set; }
        public BikeQuotationEntity BPQOutput { get; set; }
        public BikeModelEntity BikeDetails { get; set; }
        public bool IsDealerPriceAvailable { get { if (this.DPQOutput != null) { return true; } else { return false; } } }
        public bool IsInsuranceFree { get; set; }
        public uint InsuranceAmount { get; set; } 
    }
}
