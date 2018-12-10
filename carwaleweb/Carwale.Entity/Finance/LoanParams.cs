using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Finance
{
    public class LoanParams
    {
        public int LoanAmount { get; set; }
        public int Tenor { get; set; }
        public int MaxTenor { get; set; }
        public decimal LTV { get; set; }
        public decimal ROI { get; set; }
        public int ProcessingFees { get; set; }
        public bool IsPermitted { get; set; }
        public long ExShowroomPrice { get; set; }
    }
}
