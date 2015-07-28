using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    public class OtherVersionInfoEntity
    {
        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        
        public ulong OnRoadPrice { get; set; }        
    }
}
