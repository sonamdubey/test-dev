using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Details
{
    public class CarConditionApp
    {
        public string ImageHostUrl { get; set; }
        public string OverallCondition { get; set; }
        public IList<ConditionItem> CarCondition { get; set; }
    }
}
