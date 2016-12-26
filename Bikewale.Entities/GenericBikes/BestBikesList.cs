using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Entities.GenericBikes
{
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
    }
}
