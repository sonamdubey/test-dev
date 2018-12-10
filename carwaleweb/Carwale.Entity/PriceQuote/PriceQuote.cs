using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Entity.Price
{
    public class PriceQuote
    {
        public List<ChargeGroupPrice> ChargeGroup { get; private set; }
        public bool IsMetallic { get; set; }
        public long OnRoadPrice { get; set; }

        public PriceQuote()
        {
            ChargeGroup = new List<ChargeGroupPrice>();
        }
    }
}
