using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Snehal Dange on 16th Jan 2017
    /// Description: Entity created for 'research more about make ' widget on make page
    /// Created by : Sanskar Gupta on 31st Jan 2018
    /// Description : Added 'ScootersCount' to fetch the number of scooters of the current make.
    /// </summary>
    [Serializable]
    public class ResearchMoreAboutMake
    {
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase City { get; set; }
        public IEnumerable<BikeSeriesEntity> SeriesList { get; set; }
        public bool IsScooterOnlyMake { get; set; }
        public uint UsedBikesCount { get; set; }
        public uint ServiceCentersCount { get; set; }
        public uint DealerShowroomCount { get; set; }
        public uint TotalUsedBikesCount { get; set; }
        public uint TotalServiceCentersCount { get; set; }
        public uint TotalDealerShowroomCount { get; set; }
        public bool ShowServiceCenterLink { get; set; }

        public int ScootersCount { get; set; }
    }
}
