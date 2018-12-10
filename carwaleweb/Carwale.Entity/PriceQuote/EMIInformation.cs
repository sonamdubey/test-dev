using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PriceQuote
{
    [Serializable]
    public class EMIInformation
    {
        public string Text { get; set; }
        public string Amount { get; set; }
        public string TooltipMessage { get; set; }
        public string LinkText { get; set; }
        public string RupeeSymbol { get; set; }
        public string Suffix { get; set; }
    }
}
