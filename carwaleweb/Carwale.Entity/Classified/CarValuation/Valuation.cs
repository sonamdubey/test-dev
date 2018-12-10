using System;
using System.Collections.Generic;

namespace Carwale.Entity.Classified.CarValuation
{
    [Serializable]
    public class Valuation
    {
        public int Case { get; set; }
        public string Disclaimer { get; set; }
        public string WarningNote { get; set; }
        public string AvailableInfoNote { get; set; }
        public int FairPrice { get; set; }
        public int GoodPrice { get; set; }
        public List<ValuationPrices> Valuations { get; set; }
    }
}
