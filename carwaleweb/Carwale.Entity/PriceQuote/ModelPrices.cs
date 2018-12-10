using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PriceQuote
{
    /// <summary>
    /// Created By : Satish on 3 Nov 2015
    /// </summary>
    [Serializable]
    public class ModelPrices
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int CategoryId { get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryItemName { get; set; }
        public int CategoryItemValue { get; set; }
        public bool IsMetallic { get; set; }
    }
}
