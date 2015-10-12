using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By     :   Sumit Kate
    /// Modified Date   :   08 Oct 2015
    /// Desciption      :   Added the following properties
    ///                     Price,RTO,Insurance
    /// </summary>
    public class OtherVersionInfoEntity
    {
        public uint VersionId { get; set; }
        public string VersionName { get; set; }
        
        public ulong OnRoadPrice { get; set; }
        public UInt32 Price { get; set; }
        public UInt32 RTO { get; set; }
        public UInt32 Insurance { get; set; }
    }
}
