using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Dealer Offer Entity
    /// Author  :   Sumit Kate
    /// Created :   10 Sept 2015
    /// </summary>
    public class DealerOfferEntity
    {
        public uint Id { get; set; }
        public string Text { get; set; }
        public uint Value { get; set; }
        public string Type { get; set; }
    }
}
