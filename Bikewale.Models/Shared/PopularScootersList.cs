using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Shared
{
    public class PopularScootersList
    {
        public int PageCatId { get; set; }
        public int PQSourceId { get; set; }
        public IEnumerable<MostPopularBikesBase> PopularScooters { get; set; }
    }
}
