using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.UsedLeads
{
    public class UsedCarLeadCount
    {
        public int CurrentMonthLeadCount { get; set; }
        public int LastMonthLeadCount { get; set; }
        public int SecondLastMonthLeadCount { get; set; }
    }
}
