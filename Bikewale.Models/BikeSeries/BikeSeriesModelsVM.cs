using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pages;

namespace Bikewale.Models.BikeSeries
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 26th Sep 2017
    /// Modified by : VM for showing model of similar series for given model
    /// </summary>
    public class BikeSeriesModelsVM
    {
        public BikeSeriesModels SeriesModels { get; set; }
        public GAPages Page { get; set; }
        public bool IsNewAvailable { get; set; }
        public bool IsUpcomingAvailable { get; set; }
        public BikeMakeBase MakeBase { get; set; }
        public BikeModelEntityBase ModelBase { get; set; }
        public BikeSeriesEntityBase SeriesBase { get; set; }
    }
}
