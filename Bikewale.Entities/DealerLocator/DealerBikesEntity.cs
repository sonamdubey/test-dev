using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 21 March 2016
    /// Description : Dealer Deatail Entity contail bike Models available at specific dealer and dealers deatail. 
    /// </summary>
    public class DealerBikesEntity
    {
        public DealerDetailEntity DealerDetail { get; set; }
        public IEnumerable<MostPopularBikesBase> Models { get; set; }
    }
}
