using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PriceQuote
{
    /// <summary>
    /// By:Ashish Verma 
    /// </summary>
    /// 
    [Serializable]
    public class VersionsPQ
    {
        public string VersionName { get; set; }
        public string VersionId { get; set; }
        public ulong OnRoadPrice { get; set; }
        public long OnRoadPriceDiff { get; set; }
    }
}
