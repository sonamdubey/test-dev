using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store type of price and its value.
    /// </summary>
    public class PriceEntity
    {
        public string PriceType { get; set; }
        public uint PriceValue { get; set; }
    }
}
