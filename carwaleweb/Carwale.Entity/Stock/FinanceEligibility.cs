using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class FinanceEligibility
    {
        public List<int> CityIds { get; set; }
        public List<int> ModelIds { get; set; }
        public List<int> ExcludedDealerIds { get; set; }
        public List<int> ThresholdValue { get; set; } //threshold values for max age of stock in months, max number of owners allowed
    }
}
