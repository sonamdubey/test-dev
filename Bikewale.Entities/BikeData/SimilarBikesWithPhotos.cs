
using System;
namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class SimilarBikesWithPhotos
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public uint PhotosCount { get; set; }
    }
}
