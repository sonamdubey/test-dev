using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Details
{
    public class SpecificationApp
    {
        public string CategoryName { get; set; }
        public List<SpecificationItemApp> Items { get; set; }
    }
}
