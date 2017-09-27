using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 26th sep 2017
    /// Modified by : Entity to hold models of similar series
    /// </summary>
    public class BikeSeriesModels
    {
        public IEnumerable<NewBikeEntityBase> NewBikes { get; set; }
        public IEnumerable<UpcomingBikeEntityBase> UpcomingBikes { get; set; }
    }
}
