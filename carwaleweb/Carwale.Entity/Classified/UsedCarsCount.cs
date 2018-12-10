using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    /// <summary>
    /// Created By : Shalini on 05/11/14
    /// </summary>
    [Serializable]
   public class UsedCarCount
    {
        public double LiveListingCount { get; set; }
        public double MinLiveListingPrice { get; set; }
        public int ModelId { get; set; }
    }
}
