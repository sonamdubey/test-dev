using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Insurance
{
    public class InsuranceDiscount
    {
        public int Discount { get; set; }
        public int Type { get; set; }
        public bool IsVisible { get; set; }
    }
}
