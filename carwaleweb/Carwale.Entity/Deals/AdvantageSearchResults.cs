using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Deals
{
    [Serializable]
    public class AdvantageSearchResults
    {
        public List<DealsStock> Deals { get; set; }
        public int TotalCount { get; set; }
        public FilterCountEntity FilterCount { get; set; }
    }
}
