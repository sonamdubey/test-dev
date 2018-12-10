using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.UsedLeads
{
    [Serializable]
    public class DealerLeadsCount
    {
        public int VerifiedLeadCount { get; set; }
        public int UnverifiedLeadCount { get; set; }
    }
}
