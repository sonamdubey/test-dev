using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    public class BikeMakePageEntity
    {
        public BikeDescriptionEntity Description { get; set; }
        public IEnumerable<MostPopularBikesBase> PopularBikes { get; set; }
    }
}
