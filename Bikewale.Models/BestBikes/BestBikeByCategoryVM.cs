using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    public class BestBikeByCategoryVM
    {
        public IEnumerable<BestBikeEntityBase> objBestScootersList { get; set; }
        public IEnumerable<BestBikeEntityBase> objBestMileageBikesList { get; set; }
        public IEnumerable<BestBikeEntityBase> objBestSportsBikeList { get; set; }
        public IEnumerable<BestBikeEntityBase> objBestCruiserBikesList { get; set; }
        

    }
}
