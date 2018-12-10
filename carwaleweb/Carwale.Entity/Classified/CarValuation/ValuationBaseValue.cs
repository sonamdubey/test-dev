using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarValuation
{
    public class ValuationBaseValue
    {
        public double BaseValue { get; set; }
        public double NextYearBaseValue { get; set; }
        public double Deviation { get; set; }
    }
}
