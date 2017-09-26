namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 26th sep 2017
    /// Modified by : Entity to hold model of similar series
    /// </summary>
    public class SimilarBikeSeriesEntity: MinSpecsEntity
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public bool IsUpcoming { get; set; }
        public bool New { get; set; }
        public bool IsDiscontinued { get; set; }
    }
}
