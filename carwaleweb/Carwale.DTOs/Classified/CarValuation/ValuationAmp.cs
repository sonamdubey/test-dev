using System.Collections.Generic;

namespace Carwale.DTOs.Classified.CarValuation
{
    public class ValuationAmp
    {
        public int Case { get; set; }
        public string Disclaimer { get; set; }
        public string WarningNote { get; set; }
        public string AvailableInfoNote { get; set; }
        public string FairPrice { get; set; }
        public string GoodPrice { get; set; }
        public List<ValuationPricesAmp> Valuations { get; set; }
    }
}
