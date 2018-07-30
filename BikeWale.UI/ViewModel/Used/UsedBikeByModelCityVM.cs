using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by Sajal Gupta on 26-03-2017
    /// Descrioption : This class is wrapper for usedBikeByModelCity widget.
    /// </summary>
    public class UsedBikeByModelCityVM
    {
        public IEnumerable<MostRecentBikes> RecentUsedBikesList { get; set; }
        public string LinkHeading { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public CityEntityBase City { get; set; }
    }
}
