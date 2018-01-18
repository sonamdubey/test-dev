using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Snehal Dange on 16th Jan 2017
    /// Description: Entity created for 'research more about make ' widget on make page
    /// </summary>
    [Serializable]
    public class ResearchMoreAboutMake
    {
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase City { get; set; }
        public IEnumerable<BikeSeriesEntity> SeriesList { get; set; }
        public uint UsedBikesCount { get; set; }
        public bool IsScooterOnlyMake { get; set; }
        public uint ServiceCentersCount { get; set; }
    }
}
