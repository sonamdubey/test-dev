using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Entities.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 22nd DEc 2016
    /// Description : Entity for bikewale generic pages
    /// Modified by : Sajal Gupta on 02-01-2017
    /// Description : Added LastUpdatedModelSold
    /// </summary>
    [Serializable]
    public class BestBikeEntityBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string BikeName { get; set; }
        public uint TotalVersions { get; set; }
        public uint TotalModelColors { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public DateTime? LaunchDate { get; set; }
        public uint UnitsSold { get; set; }
        public uint PhotosCount { get; set; }
        public uint VideosCount { get; set; }
        public uint ExpertReviewsCount { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public uint Price { get; set; }
        public string FullModelDescription { get; set; }
        public string SmallModelDescription { get; set; }
        public string Description { get { return String.Concat(SmallModelDescription, FullModelDescription); } }
        public DateTime? LastUpdatedModelSold { get; set; }
    }
}
