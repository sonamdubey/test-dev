using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Details
{
    public class FeatureApp
    {
        public string CategoryName { get; set; }
        public List<FeatureItemApp> Items { get; set; }
    }
}
