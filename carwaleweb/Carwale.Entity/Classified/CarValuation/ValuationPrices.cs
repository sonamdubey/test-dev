
using System;
namespace Carwale.Entity.Classified.CarValuation
{
    [Serializable]
    public class ValuationPrices
    {
        public short? Year { get; set; }
        public string Version { get; set; }
        public int FairPrice { get; set; }
        public int GoodPrice { get; set; }
    }
}
