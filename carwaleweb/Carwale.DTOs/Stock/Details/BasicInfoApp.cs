using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Details
{
    public class BasicInfoApp
    {
        public List<OverviewItemApp> Overview { get; set; }
        public List<FeatureApp> Features { get; set; }
        public List<SpecificationApp> Specifications { get; set; }
    }
}
